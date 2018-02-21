using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LottieSamples.iOS
{
    [Register("ToAnimationViewController")]
    public  class ToAnimationViewController : UIViewController
    {
        private UIButton button1;

        public ToAnimationViewController() : base()
        {
        }

        protected ToAnimationViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.button1 = new UIButton(UIButtonType.System);
            this.button1.SetTitle("Show Transition B", UIControlState.Normal);
            this.button1.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.button1.BackgroundColor = new UIColor(16f / 255f, 122f / 255f, 134f / 255f, 1f);
            this.button1.Layer.CornerRadius = 7;

            this.button1.AddTarget((sender, e) => { Close(); }, UIControlEvent.TouchUpInside);
            this.View.BackgroundColor = new UIColor(red: 200f / 255f,
                                                    green: 66f / 255f,
                                                    blue: 72f / 255f,
                                                    alpha: 1f);

            this.View.AddSubview(button1);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            CGRect b = this.View.Bounds;
            CGSize buttonSize = this.button1.SizeThatFits(b.Size);
            buttonSize.Width += 20;
            buttonSize.Height += 20;

            CGRect buttonRect = new CGRect();
            buttonRect.X = b.Location.X + (0.5f * (b.Size.Width - buttonSize.Width));
            buttonRect.Y = b.Location.Y + (0.5f * (b.Size.Height - buttonSize.Height));
            buttonRect.Size = buttonSize;

            this.button1.Frame = buttonRect;
        }

        private void Close()
        {
            this.PresentingViewController.DismissViewController(true, null);
        }
    }
}
