
using ObjCRuntime;
[assembly: LinkWith ("Lottie.framework", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator | LinkTarget.Arm64 | LinkTarget.Simulator64,
Frameworks = "UIKit CoreGraphics Foundation QuartzCore",
//IsCxx = true,
SmartLink = true,
LinkerFlags="-ObjC",
ForceLoad = true)]
