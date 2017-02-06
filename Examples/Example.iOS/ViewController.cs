using System;
using Airbnb.Lottie;
using Foundation;
using UIKit;

namespace Example.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var lottieView = new LAAnimationView(NSUrl.FromString("https://raw.githubusercontent.com/airbnb/lottie-ios/master/Example/Assets/LottieLogo1.json"));
            lottieView.ContentMode = UIViewContentMode.ScaleAspectFit;
            lottieView.AutoresizingMask = UIViewAutoresizing.All;
            lottieView.Frame = AnimationView.Frame;
            lottieView.LoopAnimation = true;
            AnimationView.AddSubview(lottieView);
            Slider.Value = 0;
            Slider.ValueChanged += (sender, args) =>
            {
                lottieView.AnimationProgress = Slider.Value;
            };
            Slider.TouchUpInside += (sender, args) =>
            {
                lottieView.Play();
            };
            lottieView.Play();
        }
    }
}