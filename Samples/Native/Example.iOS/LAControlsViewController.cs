using CoreGraphics;
using System;
using UIKit;

namespace LottieSamples.iOS
{
    public partial class LAControlsViewController : UIViewController
    {
        public LAControlsViewController() : base()
        {
        }

        public LAControlsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.White;

            UIButton closeButton = new UIButton(UIButtonType.System);
            closeButton.SetTitle("Close", UIControlState.Normal);
            closeButton.TouchUpInside += CloseButton_TouchUpInside;
            this.View.AddSubview(closeButton);
            CGSize buttonSize = closeButton.SizeThatFits(this.View.Bounds.Size);
            closeButton.Frame = new CGRect(10, 30, buttonSize.Width, 50);


            /// An animated toggle with different ON and OFF animations.
            LOTAnimatedSwitch toggle1 = LOTAnimatedSwitch.SwitchNamed("Switch");
            toggle1.SetProgressRangeForOnState(0.5f, 1.0f);
            toggle1.SetProgressRangeForOffState(0f, 0.5f);

            toggle1.ValueChanged += Switch_ValueChanged;
            this.View.AddSubview(toggle1);


            /// An animated 'like' or 'heart' button.
            /// Clicking toggles the Like or Heart state.
            /// The animation runs from 0-1, progress 0 is off, progress 1 is on
            LOTAnimatedSwitch heartIcon = LOTAnimatedSwitch.SwitchNamed("TwitterHeart");
            heartIcon.ValueChanged += Switch_ValueChanged;
            this.View.AddSubview(heartIcon);


            /// This is a switch that also has a Disabled state animation.
            /// When the switch is disabled then the disabled layer is displayed.
            LOTAnimatedSwitch statefulSwitch = LOTAnimatedSwitch.SwitchNamed("Switch_States");
            statefulSwitch.SetProgressRangeForOnState(1f, 0f);
            statefulSwitch.SetProgressRangeForOffState(0f, 1f);

            statefulSwitch.SetLayerName("Button", UIControlState.Normal);
            statefulSwitch.SetLayerName("Disabled", UIControlState.Disabled);

            // Changes visual appearance by switching animation layer to "Disabled"
            statefulSwitch.Enabled = false;

            // Changes visual appearance by switching animation layer to "Button"
            statefulSwitch.Enabled = true;

            statefulSwitch.ValueChanged += Switch_ValueChanged;

            statefulSwitch.ValueChanged += Switch_ValueChanged;
            this.View.AddSubview(statefulSwitch);

            // Layout
            toggle1.Center = new CGPoint(this.View.Bounds.GetMidX(), 90);
            heartIcon.Bounds = new CGRect(0, 0, 200, 200);
            heartIcon.Center = new CGPoint(this.View.Bounds.GetMidX(), toggle1.Frame.GetMaxY() + (heartIcon.Bounds.Size.Height * 0.5));
            statefulSwitch.Center = new CGPoint(this.View.Bounds.GetMidX(), heartIcon.Frame.GetMaxY() + (statefulSwitch.Bounds.Size.Height * 0.5));

        }

        private void Switch_ValueChanged(object sender, EventArgs e)
        {
            var animatedSwitch = sender as LOTAnimatedSwitch;
            Console.WriteLine("The switch is " + (animatedSwitch.On ? "ON" : "OFF"));
        }

        private void CloseButton_TouchUpInside(object sender, EventArgs e)
        {
            this.PresentingViewController.DismissViewController(true, null);
        }
    }
}

