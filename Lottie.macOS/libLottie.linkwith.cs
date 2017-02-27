using ObjCRuntime;

[assembly: LinkWith ("libLottie.a", 
Frameworks = "AppKit",
IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
