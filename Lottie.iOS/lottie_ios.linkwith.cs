using ObjCRuntime;
[assembly: LinkWith ("lottie_ios.a", 
Frameworks = "UIKit",
IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
