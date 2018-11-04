using ObjCRuntime;

[assembly: LinkWith ("libLottie-macos.a", 
Frameworks = "AppKit",
IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
