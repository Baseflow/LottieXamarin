using System;
using Airbnb.Lottie;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LottieSamples.iOS
{
    internal partial class AnimationTransitionViewController : UIViewController, IUIViewControllerTransitioningDelegate
    {
        private UIButton button1;
        private UIButton closeButton;


        public AnimationTransitionViewController() : base()
        {
        }

        protected AnimationTransitionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.closeButton = new UIButton(UIButtonType.System);
            this.closeButton.SetTitle("Close", UIControlState.Normal);
            this.closeButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.closeButton.BackgroundColor = new UIColor(50f / 255f, 207f / 255f, 193f / 255f, 1f);
            this.closeButton.Layer.CornerRadius = 7;
            this.closeButton.AddTarget((sender, e) => { Close(); }, UIControlEvent.TouchUpInside);
            this.View.AddSubview(closeButton);



            this.button1 = new UIButton(UIButtonType.System);
            this.button1.SetTitle("Show Transition A", UIControlState.Normal);
            this.button1.SetTitleColor(UIColor.White, UIControlState.Normal);
            this.button1.BackgroundColor = new UIColor(50f / 255f, 207f / 255f, 193f / 255f, 1f);
            this.button1.Layer.CornerRadius = 7;
            this.button1.AddTarget((sender, e) => { ShowTransitionA(); }, UIControlEvent.TouchUpInside);
            this.View.AddSubview(button1);


            this.View.BackgroundColor = new UIColor(red: 122f / 255f,
                                                    green: 8f / 255f,
                                                    blue: 81f / 255f,
                                                    alpha: 1f);
        }


        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            CGRect b = this.View.Bounds;
            CGSize buttonSize = this.button1.SizeThatFits(b.Size);
            buttonSize.Width += 20;
            buttonSize.Height += 20;
            this.button1.Bounds = new CGRect(0, 0, buttonSize.Width, buttonSize.Height);
            this.button1.Center = this.View.Center;


            CGSize closeSize = this.closeButton.SizeThatFits(b.Size);
            closeSize.Width += 20;
            closeSize.Height += 20;
            this.closeButton.Bounds = new CGRect(0, 0, closeSize.Width, closeSize.Height);
            this.closeButton.Center = new CGPoint(this.button1.Center.X, b.GetMaxY() - closeSize.Height);
        }

        private void ShowTransitionA()
        {
            ToAnimationViewController vc = new ToAnimationViewController();
            vc.TransitioningDelegate = this;
            this.PresentViewController(vc, animated: true, completionHandler: null);
        }

        private void Close()
        {
            this.PresentingViewController.DismissViewController(animated: true, completionHandler: null);
        }

        #region View Controller Transitioning
        [Export("animationControllerForPresentedController:presentingController:sourceController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            LOTAnimationTransitionController animationController = new LOTAnimationTransitionController(
                animation: "vcTransition1",
                fromLayer: "outLayer",
                toLayer: "inLayer",
                applyAnimationTransform: false
            );

            return animationController;
        }


        [Export("animationControllerForDismissedController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            LOTAnimationTransitionController animationController = new LOTAnimationTransitionController(
               animation: "vcTransition2",
               fromLayer: "outLayer",
               toLayer: "inLayer",
               applyAnimationTransform: false
            );


            return animationController;
        }

        #endregion

    }
}