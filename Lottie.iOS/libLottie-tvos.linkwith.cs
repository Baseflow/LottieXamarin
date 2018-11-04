using ObjCRuntime;
[assembly: LinkWith ("libLottie-tvos.a", 
Frameworks = "UIKit",
IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
