using CoreGraphics;
using Foundation;
using UIKit;

namespace LottieSamples.iOS
{
    public partial class TypingDemoViewController : UIViewController, IUITextFieldDelegate
    {
        private UIButton closeButton;
        private UITextField typingField;
        private UISlider fontSlider;
        private AnimatedTextField textField;
        private NSObject kbShowNotification;
        private NSObject kbHideNotification;


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.White;

            this.closeButton = new UIButton(UIButtonType.System);
            this.closeButton.SetTitle("Close", UIControlState.Normal);
            this.closeButton.TouchUpInside += (sender, e) =>
            {
                this.typingField.ResignFirstResponder();
                this.PresentingViewController.DismissViewController(true, null);
            };
            this.View.AddSubview(this.closeButton);

            this.textField = new AnimatedTextField(this.View.Bounds);
            this.textField.Text = "Start Typing";
            this.View.AddSubview(this.textField);

            this.typingField = new UITextField(CGRect.Empty);
            this.typingField.Alpha = 0f;
            this.typingField.Text = this.textField.Text;
            this.typingField.Delegate = this;
            this.View.AddSubview(this.typingField);

            this.fontSlider = new UISlider(CGRect.Empty);
            this.fontSlider.MinValue = 18;
            this.fontSlider.MaxValue = 128;
            this.fontSlider.Value = 36;
            this.fontSlider.ValueChanged += (sender, e) => this.textField.FontSize = (int)this.fontSlider.Value;
            this.View.AddSubview(this.fontSlider);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.kbShowNotification = UIKeyboard.Notifications.ObserveWillShow(KeyboardChangeHandler);
            this.kbHideNotification = UIKeyboard.Notifications.ObserveWillHide(KeyboardChangeHandler);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.kbHideNotification.Dispose();
            this.kbShowNotification.Dispose();
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.typingField.BecomeFirstResponder();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            CGSize buttonSize = this.closeButton.SizeThatFits(this.View.Bounds.Size);
            this.closeButton.Frame = new CGRect(10, 30, buttonSize.Width, 50);
            this.fontSlider.Frame = new CGRect(10, closeButton.Frame.GetMaxY(), this.View.Bounds.Size.Width - 20, 44);
            this.textField.Frame = new CGRect(0, this.fontSlider.Frame.GetMaxY(),
                                              this.View.Bounds.Size.Width,
                                              this.View.Bounds.Size.Height - this.fontSlider.Frame.GetMaxY());
            this.typingField.Frame = new CGRect(0, -100, this.View.Bounds.Size.Width, 25);
        }


        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharactersInRange(UITextField textField, NSRange range, string replacementString)
        {
            this.textField.ChangeCharactersInRange(range, replacementString);
            return true;
        }


        private void KeyboardChangeHandler(object sender, UIKeyboardEventArgs e)
        {
            this.textField.SetScrollInsets(new UIEdgeInsets(0, 0, e.FrameEnd.Size.Height, 0));
        }
    }
}