using ObjCRuntime;
[assembly: LinkWith("libLottie-ios.a",
Frameworks = "UIKit",
IsCxx = true,
SmartLink = true,
LinkerFlags = "-ObjC",
ForceLoad = true)]
