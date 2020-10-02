using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using ZXing.Mobile;

namespace LottieSamples.iOS
{
    public partial class AnimationExplorerViewController : UIViewController
    {
        private enum ViewBackgroundColorEnum
        {
            White,
            Black,
            Green,
            None
        }


        private ViewBackgroundColorEnum backgroundColor;
        private UIToolbar toolbar;
        private UISlider slider;
        private LOTAnimationView laAnimation;

        public AnimationExplorerViewController() : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.SetBackgroundColor(ViewBackgroundColorEnum.White);

            this.laAnimation = new LOTAnimationView();
            this.toolbar = new UIToolbar(CGRect.Empty);

            UIBarButtonItem open = new UIBarButtonItem(UIBarButtonSystemItem.Bookmarks, OpenEventHandler);
            UIBarButtonItem flx1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem openWeb = new UIBarButtonItem(UIBarButtonSystemItem.Action, RemoteJsonEventHandler);
            UIBarButtonItem flx2 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem play = new UIBarButtonItem(UIBarButtonSystemItem.Play, PlayEventHandler);
            UIBarButtonItem flx3 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem loop = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, LoopEventHandler);
            UIBarButtonItem flx4 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem zoom = new UIBarButtonItem(UIBarButtonSystemItem.Add, ZoomEventHandler);
            UIBarButtonItem flx5 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem bgcolor = new UIBarButtonItem(UIBarButtonSystemItem.Compose, ChangeBackgroundColorEventHandler);
            UIBarButtonItem flx6 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, handler: null);

            UIBarButtonItem close = new UIBarButtonItem(UIBarButtonSystemItem.Stop, CloseEventHandler);

            this.toolbar.Items = new UIBarButtonItem[] { open, flx1, openWeb, flx2, loop, flx3, play, flx4, zoom, flx5, bgcolor, flx6, close };
            this.View.AddSubview(toolbar);

            this.slider = new UISlider(CGRect.Empty);
            this.slider.ValueChanged += (sender, e) => this.laAnimation.AnimationProgress = this.slider.Value; ;
            this.slider.MinValue = 0f;
            this.slider.MaxValue = 1f;
            this.View.AddSubview(this.slider);

            this.ResetAllButtons();
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            CGRect b = this.View.Bounds;
            this.toolbar.Frame = new CGRect(0, b.Size.Height - 44, b.Size.Width, 44);
            CGSize sliderSize = this.slider.SizeThatFits(b.Size);

            sliderSize.Height += 12;

            this.slider.Frame = new CGRect(10, this.toolbar.Frame.GetMinY() - sliderSize.Height,
                                           b.Size.Width - 20, sliderSize.Height);
            this.laAnimation.Frame = new CGRect(0, 0, b.Size.Width, this.slider.Frame.GetMinY());
        }

        private void OpenEventHandler(object sender, EventArgs e)
        {
            this.ShowJsonExplorer();
        }

        private void RemoteJsonEventHandler(object sender, EventArgs e)
        {
            UIAlertController actionSheet = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            actionSheet.AddAction(UIAlertAction.Create("Load animation from URL", UIAlertActionStyle.Default, (action) => ShowUrlInput()));
            actionSheet.AddAction(UIAlertAction.Create("Scan LottieFiles QR Code", UIAlertActionStyle.Default, async (action) => await ShowQRCodeScannerAsync()));
            actionSheet.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            this.PresentViewController(actionSheet, true, null);
        }

        private async void ZoomEventHandler(object sender, EventArgs e)
        {
            switch (this.laAnimation.ContentMode)
            {
                case UIViewContentMode.ScaleAspectFit:
                    this.laAnimation.ContentMode = UIViewContentMode.ScaleAspectFill;
                    await this.ShowMessageAsync("Aspect Fill");
                    break;
                case UIViewContentMode.ScaleAspectFill:
                    this.laAnimation.ContentMode = UIViewContentMode.ScaleToFill;
                    await this.ShowMessageAsync("Scale Fill");
                    break;
                case UIViewContentMode.ScaleToFill:
                    this.laAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
                    await this.ShowMessageAsync("Aspect Fit");
                    break;
                default:
                    break;
            }
        }

        private void ChangeBackgroundColorEventHandler(object sender, EventArgs e)
        {
            ViewBackgroundColorEnum currentBg = this.backgroundColor;
            currentBg += 1;
            if (currentBg == ViewBackgroundColorEnum.None)
            {
                currentBg = ViewBackgroundColorEnum.White;
            }

            this.SetBackgroundColor(currentBg);
        }


        private void ShowUrlInput()
        {
            UIAlertController alert = UIAlertController.Create("Load From URL", null, UIAlertControllerStyle.Alert);
            alert.AddTextField((UITextField obj) => obj.Placeholder = "Enter URL");

            UIAlertAction load = UIAlertAction.Create("Load", UIAlertActionStyle.Default, (obj) => this.LoadAnimationFromUrl(alert.TextFields.ToString()));
            alert.AddAction(load);

            UIAlertAction close = UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null);
            alert.AddAction(close);

            this.PresentViewController(alert, animated: true, completionHandler: null);
        }

        private async Task ShowQRCodeScannerAsync()
        {
            MobileBarcodeScanningOptions options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE };

            MobileBarcodeScanner scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
            scanner.TopText = "Scan QR Code from lottiefiles.com";
            scanner.AutoFocus();

            ZXing.Result result = await scanner.Scan(options, true);
            if (result != null)
                LoadAnimationFromUrl(result.Text);

        }

        private void ShowJsonExplorer()
        {
            JSONExplorerViewController vc = new JSONExplorerViewController();
            vc.CompletionBlock = (string fileName) =>
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    this.LoadAnimationNamed(fileName);
                }
                this.DismissViewController(animated: true, completionHandler: null);
            };

            UINavigationController navController = new UINavigationController(vc);
            this.PresentViewController(navController, animated: true, completionHandler: null);
        }

        private void LoadAnimationNamed(string named)
        {
            this.laAnimation.RemoveFromSuperview();
            this.laAnimation = null;
            this.ResetAllButtons();

            this.laAnimation = LOTAnimationView.AnimationNamed(named);
            this.laAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.View.AddSubview(this.laAnimation);
            this.View.SetNeedsLayout();

        }

        private void LoadAnimationFromUrl(string url)
        {
            this.laAnimation.RemoveFromSuperview();
            this.laAnimation = null;
            this.ResetAllButtons();

            this.laAnimation = new LOTAnimationView(new Foundation.NSUrl(url));
            this.laAnimation.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.View.AddSubview(laAnimation);
            this.View.SetNeedsLayout();
        }

        private void ResetAllButtons()
        {
            this.slider.Value = 0f;
            foreach (UIBarButtonItem button in this.toolbar.Items)
            {
                ResetButton(button, highlighted: false);
            }
        }

        private void ResetButton(UIBarButtonItem button, bool highlighted)
        {
            button.TintColor = highlighted ? UIColor.Red : new UIColor(red: 50f / 255f,
                                                                       green: 207f / 255f,
                                                                       blue: 193f / 255f,
                                                                       alpha: 1f);
        }

        private void SetBackgroundColor(ViewBackgroundColorEnum color)
        {
            this.backgroundColor = color;

            switch (backgroundColor)
            {
                case ViewBackgroundColorEnum.White:
                    this.View.BackgroundColor = UIColor.White;
                    break;
                case ViewBackgroundColorEnum.Black:
                    this.View.BackgroundColor = UIColor.Black;
                    break;
                case ViewBackgroundColorEnum.Green:
                    this.View.BackgroundColor = new UIColor(red: 50f / 255f,
                                                            green: 207f / 255f,
                                                            blue: 193f / 255f,
                                                            alpha: 1f);
                    break;
                case ViewBackgroundColorEnum.None:
                    this.View.BackgroundColor = null;
                    break;
                default:
                    break;
            }
        }

        private async void LoopEventHandler(object sender, EventArgs e)
        {
            UIBarButtonItem button = sender as UIBarButtonItem;
            this.laAnimation.LoopAnimation = !this.laAnimation.LoopAnimation;
            this.ResetButton(button, highlighted: this.laAnimation.LoopAnimation);
            await this.ShowMessageAsync(this.laAnimation.LoopAnimation ? "Loop Enabled" : "Loop Disabled");
        }

        private void CloseEventHandler(object sender, EventArgs e)
        {
            this.PresentingViewController.DismissViewController(animated: true, completionHandler: null);
        }

        private void PlayEventHandler(object sender, EventArgs e)
        {
            UIBarButtonItem button = sender as UIBarButtonItem;

            if (this.laAnimation.IsAnimationPlaying)
            {
                this.ResetButton(button, highlighted: false);
                this.laAnimation.Pause();
            }
            else
            {
                CADisplayLink displayLink = CADisplayLink.Create(UpdateProgressSlider);
                displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Common);
                this.ResetButton(button, highlighted: true);
                this.laAnimation.PlayWithCompletion((arg0) =>
                {
                    displayLink.Invalidate();
                    this.ResetButton(button, highlighted: false);
                });
            }

        }

        private async Task ShowMessageAsync(string message)
        {
            UILabel messageLabel = new UILabel(CGRect.Empty);
            messageLabel.TextAlignment = UITextAlignment.Center;
            messageLabel.BackgroundColor = UIColor.Black.ColorWithAlpha(0.3f);
            messageLabel.TextColor = UIColor.White;
            messageLabel.Text = message;

            CGSize messageSize = messageLabel.SizeThatFits(this.View.Bounds.Size);
            messageSize.Width += 14;
            messageSize.Height += 14;
            messageLabel.Frame = new CGRect(10, 30, messageSize.Width, messageSize.Height);
            messageLabel.Alpha = 0f;
            this.View.AddSubview(messageLabel);

            await UIView.AnimateAsync(0.3f, () => messageLabel.Alpha = 1f);
            UIView.Animate(0.3f, 1f, UIViewAnimationOptions.CurveEaseInOut,
                           () => messageLabel.Alpha = 0f,
                           () => messageLabel.RemoveFromSuperview());
        }

        private void UpdateProgressSlider()
        {
            this.slider.Value = (float)this.laAnimation.AnimationProgress;
        }
    }
}

