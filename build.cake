#tool nuget:?package=GitVersion.CommandLine&version=5.0.1
#tool nuget:?package=vswhere&version=2.7.1
#addin nuget:?package=Cake.Figlet&version=1.3.1
#addin nuget:?package=Cake.Git&version=0.21.0
#addin nuget:?package=Polly&version=7.1.1

using Polly;

var solutionName = "Lottie";
var repoName = "martijn00/LottieXamarin";
var sln = new FilePath("./" + solutionName + ".sln");
var outputDir = new DirectoryPath("./artifacts");
var gitVersionLog = new FilePath("./artifacts/gitversion.log");
var nuspecDir = new DirectoryPath("./nuspec");
var nugetPackagesDir = new DirectoryPath("./nuget/packages");
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosityArg = Argument("verbosity", "Minimal");
var verbosity = Verbosity.Minimal;

var signingSecret = EnvironmentVariable("SIGNING_SECRET");
var signingUser = EnvironmentVariable("SIGNING_USER");
var didSignPackages = false;

var nugetSource = EnvironmentVariable("NUGET_SOURCE");
var nugetApiKey = EnvironmentVariable("NUGET_APIKEY");

var githubToken = EnvironmentVariable("GITHUB_TOKEN");
var githubTokenEnv = EnvironmentVariable("CHANGELOG_GITHUB_TOKEN");
var sinceTag = Argument("since_tag", "");

var shouldPublishPackages = false;

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
GitVersion versionInfo = null;

Setup(context => 
{
    versionInfo = context.GitVersion(new GitVersionSettings 
	{
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json,
        LogFilePath = gitVersionLog.MakeAbsolute(context.Environment)
    });

    if (isRunningOnAppVeyor)
    {
		var buildNumber = versionInfo.InformationalVersion + "-" + AppVeyor.Environment.Build.Number;
        buildNumber = buildNumber.Replace("/", "-");
        AppVeyor.UpdateBuildVersion(buildNumber);
    }

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information(Figlet(solutionName));
    Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
        versionInfo.SemVer,
        configuration,
        target,
        cakeVersion);
		
	shouldPublishPackages = ShouldPushNugetPackages(versionInfo.BranchName, nugetSource);

    Information("Will push NuGet packages {0}", shouldPublishPackages);

    verbosity = (Verbosity) Enum.Parse(typeof(Verbosity), verbosityArg, true);
});

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    CleanDirectories(outputDir.FullPath);
    CleanDirectories(nugetPackagesDir.FullPath);

    EnsureDirectoryExists(outputDir);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
    .WithCriteria(() => IsRunningOnWindows())
    .Does(() => 
{
    var vsWhereSettings = new VSWhereLatestSettings
    {
        IncludePrerelease = true,
        Requires = "Component.Xamarin"
    };
    
    var vsLatest = VSWhereLatest(vsWhereSettings);
    msBuildPath = (vsLatest == null)
        ? null
        : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");

    if (msBuildPath != null)
        Information("Found MSBuild at {0}", msBuildPath.ToString());
});

Task("Restore")
    .IsDependentOn("ResolveBuildTools")
    .Does(() => 
{
    var settings = GetDefaultBuildSettings()
        .WithTarget("Restore");
    MSBuild(sln, settings);
});

Task("PatchBuildProps")
    .Does(() => 
{
    var buildProp = new FilePath("./Directory.build.props");
    XmlPoke(buildProp, "//Project/PropertyGroup/Version", versionInfo.SemVer);
});

Task("Build")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("Clean")
    .IsDependentOn("PatchBuildProps")
    .IsDependentOn("Restore")
    .Does(() =>  {

    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithProperty("NoPackageAnalysis", "True")
        .WithTarget("Build");
	
    MSBuild(sln, settings);
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(outputDir + "/Tests/");

    var testPaths = GetFiles("./UnitTests/*.UnitTest/*.UnitTest.csproj");
    var testsFailed = false;

    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    foreach(var project in testPaths)
    {
        var projectName = project.GetFilenameWithoutExtension();
        var testXml = MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".xml"));
        settings.Logger = $"xunit;LogFilePath={testXml.FullPath}";
        try 
        {
            DotNetCoreTest(project.ToString(), settings);
        }
        catch
        {
            testsFailed = true;
        }
    }

    if (isRunningOnAppVeyor)
    {
        foreach(var testResult in GetFiles(outputDir + "/Tests/*.xml"))
            AppVeyor.UploadTestResults(testResult, AppVeyorTestResultsType.XUnit);
    }

    if (testsFailed)
        throw new Exception("Tests failed :(");
});

Task("CopyPackages")
    .IsDependentOn("Build")
    .Does(() => 
{
    var nugetFiles = GetFiles(solutionName + "*/**/bin/" + configuration + "/**/*.nupkg");
    CopyFiles(nugetFiles, outputDir);
});

Task("SignPackages")
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .WithCriteria(() => IsRepository(repoName))
    .WithCriteria(() => !string.IsNullOrEmpty(signingSecret))
    .WithCriteria(() => !string.IsNullOrEmpty(signingUser))
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !AppVeyor.Environment.PullRequest.IsPullRequest)
    .IsDependentOn("Build")
    .IsDependentOn("CopyPackages")
    .Does(() => 
{
    var settings = File("./signclient.json");
    var files = GetFiles(outputDir + "/*.nupkg");

    foreach(var file in files)
    {
        Information("Signing {0}...", file.FullPath);

        // Build the argument list.
        var arguments = new ProcessArgumentBuilder()
            .Append("sign")
            .AppendSwitchQuoted("-c", MakeAbsolute(settings.Path).FullPath)
            .AppendSwitchQuoted("-i", MakeAbsolute(file).FullPath)
            .AppendSwitchQuotedSecret("-s", signingSecret)
            .AppendSwitchQuotedSecret("-r", signingUser)
            .AppendSwitchQuoted("-n", solutionName)
            .AppendSwitchQuoted("-d", solutionName)
            .AppendSwitchQuoted("-u", "https://baseflow.com");

        // Sign the binary.
        var result = StartProcess("SignClient.exe", new ProcessSettings { Arguments = arguments });
        if (result != 0)
        {
            // We should not recover from this.
            throw new InvalidOperationException("Signing failed!");
        }
    }

    didSignPackages = true;
});

Task("PublishPackages")
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .WithCriteria(() => IsRepository(repoName))
    .WithCriteria(() => !string.IsNullOrEmpty(nugetSource))
    .WithCriteria(() => !string.IsNullOrEmpty(nugetApiKey))
    .WithCriteria(() => shouldPublishPackages)
    .IsDependentOn("CopyPackages")
    //.IsDependentOn("SignPackages")
    .Does (() =>
{
    //if (!didSignPackages)
    //{
    //    Warning("Packages were not signed. Not publishing packages");
    //    return;
    //}

    var nugetPushSettings = new NuGetPushSettings
    {
        Source = nugetSource,
        ApiKey = nugetApiKey
    };

    if (nugetSource.Contains("github.com"))
    {
        var nugetSourceSettings = new NuGetSourcesSettings
        {
            UserName = "Martijn00",
            Password = githubToken,
            IsSensitiveSource = true
        };

        var feed = new 
        {
            Name = "GitHub",
            Source = nugetSource
        };

        NuGetAddSource(feed.Name, feed.Source, nugetSourceSettings);

        nugetPushSettings = new NuGetPushSettings
        {
            Source = feed.Source
        };
    }

    var nugetFiles = GetFiles(outputDir + "/*.nupkg");

    var policy = Policy
        .Handle<Exception>()
        .WaitAndRetry(5, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(1.5, retryAttempt)));

    foreach(var nugetFile in nugetFiles)
    {
        policy.Execute(() =>
            NuGetPush(nugetFile, nugetPushSettings)
        );
    }
});

Task("UploadArtifacts")
    .IsDependentOn("CopyPackages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() => 
{
	Information("Artifacts Dir: {0}", outputDir.FullPath);

    var uploadSettings = new AppVeyorUploadArtifactsSettings();

    var artifacts = GetFiles(outputDir.FullPath + "/**/*");

    foreach(var file in artifacts) {
        Information("Uploading {0}", file.FullPath);

        if (file.GetExtension().Contains("nupkg"))
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.NuGetPackage;
        else
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.Auto;

        AppVeyor.UploadArtifact(file.FullPath, uploadSettings);
    }
	
});

Task("UpdateChangelog")
    .Does(() => 
{
    var arguments = new ProcessArgumentBuilder();
    if (!string.IsNullOrEmpty(githubToken))
        arguments.Append("--token {0}", githubToken);
    else if (!string.IsNullOrEmpty(githubTokenEnv))
        arguments.Append("--token {0}", githubTokenEnv);

    // Exclude labels
    var excludeLabels = new [] {
        "t/question",
        "s/wont-fix",
        "s/duplicate",
        "s/deprecated",
        "s/invalid"
    };
    arguments.Append("--exclude-labels {0}", string.Join(",", excludeLabels));

    // bug labels
    arguments.Append("--bug-labels {0}", "t/bug");

    // enhancement labels
    arguments.Append("--enhancement-labels {0}", "t/feature");

    // breaking labels (enable when github_changelog_generator 1.15 is released)
    //arguments.Append("--breaking-labels {0}", "t/breaking");

    arguments.Append("--max-issues 200");

    if (!string.IsNullOrEmpty(sinceTag) && versionInfo.BranchName.Contains("release/"))
    {
        arguments.Append("--between-tags {0},{1}", sinceTag, versionInfo.MajorMinorPatch);

        arguments.Append("--future-release {0}", versionInfo.MajorMinorPatch);
    }
    else 
    {
        if (!string.IsNullOrEmpty(sinceTag))
            arguments.Append("--since-tag {0}", sinceTag);

        if (versionInfo.BranchName.Contains("release/"))
            arguments.Append("--future-release {0}", versionInfo.MajorMinorPatch);
    }

    Information("Starting github_changelog_generator with arguments: {0}", arguments.Render());

    using(var process = StartAndReturnProcess("github_changelog_generator",
        new ProcessSettings { Arguments = arguments }))
    {
        process.WaitForExit();
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .IsDependentOn("UploadArtifacts")
    .IsDependentOn("PublishPackages")
    .Does(() => 
{
});

RunTarget(target);

MSBuildSettings GetDefaultBuildSettings()
{
    var settings = new MSBuildSettings 
    {
        Configuration = configuration,
        ToolPath = msBuildPath,
        Verbosity = verbosity,
        ArgumentCustomization = args => args.Append("/m"),
        ToolVersion = MSBuildToolVersion.VS2022
    };

    return settings;
}

bool ShouldPushNugetPackages(string branchName, string nugetSource)
{
    if (StringComparer.OrdinalIgnoreCase.Equals(branchName, "develop"))
        return true;

    var masterOrRelease = IsMasterOrReleases(branchName);

    if (masterOrRelease && nugetSource != null && nugetSource.Contains("github.com"))
        return true;

    return masterOrRelease && IsTagged().Item1;
}

bool IsMasterOrReleases(string branchName)
{
    if (StringComparer.OrdinalIgnoreCase.Equals(branchName, "master"))
        return true;

    if (branchName.StartsWith("release/", StringComparison.OrdinalIgnoreCase) ||
        branchName.StartsWith("releases/", StringComparison.OrdinalIgnoreCase))
        return true;

    return false;
}

bool IsRepository(string repoName)
{
    if (isRunningOnAppVeyor)
    {
        var buildEnvRepoName = AppVeyor.Environment.Repository.Name;
        Information("Checking repo name: {0} against build repo name: {1}", repoName, buildEnvRepoName);
        return StringComparer.OrdinalIgnoreCase.Equals(repoName, buildEnvRepoName);
    }
    else
    {
        try
        {
            var path = MakeAbsolute(sln).GetDirectory().FullPath;
            using (var repo = new LibGit2Sharp.Repository(path))
            {
                var origin = repo.Network.Remotes.FirstOrDefault(
                    r => r.Name.ToLowerInvariant() == "origin");
                return origin.Url.ToLowerInvariant() == 
                    "https://github.com/" + repoName.ToLowerInvariant();
            }
        }
        catch(Exception ex)
        {
            Information("Failed to lookup repository: {0}", ex);
            return false;
        }
    }
}

Tuple<bool, string> IsTagged()
{
    var path = MakeAbsolute(sln).GetDirectory().FullPath;
    using (var repo = new LibGit2Sharp.Repository(path))
    {
        var head = repo.Head;
        var headSha = head.Tip.Sha;
        
        var tag = repo.Tags.FirstOrDefault(t => t.Target.Sha == headSha);
        if (tag == null)
        {
            Debug("HEAD is not tagged");
            return Tuple.Create<bool, string>(false, null);
        }

        Debug("HEAD is tagged: {0}", tag.FriendlyName);
        return Tuple.Create<bool, string>(true, tag.FriendlyName);
    }
}
