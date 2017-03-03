using ObjCRuntime;
[assembly: LinkWith ("libLottie.a", 
Frameworks = "UIKit",
IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
