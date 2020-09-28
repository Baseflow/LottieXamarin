using System;
using Airbnb.Lottie;
using UIKit;

namespace Example.tvOS
{
    public partial class ViewController : UIViewController
    {
        private LOTAnimationView lottieLogo;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.lottieLogo = LOTAnimationView.AnimationNamed("LottieLogo1");
            this.lottieLogo.ContentMode = UIViewContentMode.ScaleAspectFill;
            this.lottieLogo.Frame = this.View.Bounds;
            this.lottieLogo.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            this.lottieLogo.LoopAnimation = true;
            this.View.AddSubview(this.lottieLogo);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.lottieLogo.Play();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.lottieLogo.Pause();
        }
    }
}

