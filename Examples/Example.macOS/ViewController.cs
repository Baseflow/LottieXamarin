using System;

using AppKit;

namespace Example.macOS
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.rewindButton.Activated += (sender, e) =>
                (this.View as LAMainView).RewindAnimation();

            this.loopButton.Activated += (sender, e) => 
                (this.View as LAMainView).ToogleLoop();

            this.playButton.Activated += (sender, e) => 
                (this.View as LAMainView).PlayAnimation();

            this.progressSlider.Activated  += (object sender, EventArgs e) => {
                (this.View as LAMainView).AnimationProgress = this.progressSlider.FloatValue;
            };

        }
    }
}
