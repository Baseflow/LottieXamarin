using System;

using AppKit;
using Foundation;

using Airbnb.Lottie;

namespace Example.macOS
{
    public partial class ViewController : NSViewController
    {
        private LOTAnimationView lottieLogo;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.lottieLogo = LOTAnimationView.AnimationNamed("LottieLogo1");
            this.lottieLogo.ContentMode = LOTViewContentMode.ScaleAspectFill;
            this.lottieLogo.Frame = this.View.Bounds;
            this.lottieLogo.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
            this.lottieLogo.LoopAnimation = true;
            this.View.AddSubview(this.lottieLogo);
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            this.lottieLogo.Play();
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            this.lottieLogo.Pause();
        }
    }
}
