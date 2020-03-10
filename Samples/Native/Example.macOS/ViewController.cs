using System;

using AppKit;
using Foundation;
using ObjCRuntime;

namespace Example.macOS
{
    public partial class ViewController : NSViewController
    {
        private LAMainView lottieView;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.lottieView = this.View as LAMainView;

            this.rewindButton.Activated += (sender, e) =>
                this.lottieView.RewindAnimation();

            this.loopButton.Activated += (sender, e) =>
                this.lottieView.ToogleLoop();

            this.playButton.Activated += (sender, e) =>
                this.lottieView.PlayAnimation();

            this.progressSlider.Activated += (object sender, EventArgs e) =>
            {
                this.lottieView.AnimationProgress = this.progressSlider.FloatValue;
            };
        }

        [Export("paste:")]
        public void Paste(NSObject sender)
        {
            var pasteboard = NSPasteboard.GeneralPasteboard;
            var classArray = new[] { new Class(typeof(NSUrl)) };

            var validContent = pasteboard.CanReadObjectForClasses(classArray, null);
            if (validContent)
            {
                var copiedItems = pasteboard.ReadObjectsForClasses(classArray, null);
                var url = (NSUrl)copiedItems[0];

                if (LottieFilesUrl.IsValidUrl(url))
                {
                    var lottieUrl = new LottieFilesUrl(url);
                    this.View.Window.Title = lottieUrl.AnimationName;
                    this.lottieView.OpenAnimationUrl(lottieUrl.JsonUrl);
                }
            }
        }
    }
}
