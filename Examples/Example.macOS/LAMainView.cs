using System;
using Foundation;
using AppKit;
using System.Linq;
using System.IO;
using Airbnb.Lottie;


namespace Example.macOS
{
    public partial class LAMainView : NSView
    {
        private LOTAnimationView lottieLogo;

        #region Constructors

        // Called when created from unmanaged code
        public LAMainView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public LAMainView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
            RegisterForDraggedTypes(new string[] { NSPasteboard.NSFilenamesType });

            this.lottieLogo = LOTAnimationView.AnimationNamed("LottieLogo1");
            this.lottieLogo.ContentMode = LOTViewContentMode.ScaleAspectFill;
            this.lottieLogo.Frame = this.Bounds;
            this.lottieLogo.AutoresizingMask = NSViewResizingMask.WidthSizable |
                NSViewResizingMask.HeightSizable;

            this.lottieLogo.Layer.ZPosition = -10000;
            this.AddSubview(this.lottieLogo);
        }

        #endregion

        public override void ViewDidMoveToSuperview()
        {
            base.ViewDidMoveToSuperview();
            this.lottieLogo.Play();
        }

        public override NSDragOperation DraggingEntered(NSDraggingInfo sender)
        {
            NSPasteboard pboard;
            NSDragOperation sourceDragMask;

            sourceDragMask = sender.DraggingSourceOperationMask;
            pboard = sender.DraggingPasteboard;

            if (Array.IndexOf(pboard.Types, NSPasteboard.NSColorType) >= 0 && sourceDragMask.HasFlag(NSDragOperation.Generic))
            {
                return NSDragOperation.Generic;
            }

            if (Array.IndexOf(pboard.Types, NSPasteboard.NSFilenamesType) >= 0)
            {
                if (sourceDragMask.HasFlag(NSDragOperation.Link))
                {
                    return NSDragOperation.Generic;
                }
                else
                {
                    return NSDragOperation.Copy;
                }
            }


            return NSDragOperation.None;
        }

        public override bool PerformDragOperation(NSDraggingInfo sender)
        {
            NSPasteboard pboard;
            NSDragOperation sourceDragMask;

            sourceDragMask = sender.DraggingSourceOperationMask;
            pboard = sender.DraggingPasteboard;

            if (Array.IndexOf(pboard.Types, NSPasteboard.NSColorType) > -1)
            {

            }
            else if (Array.IndexOf(pboard.Types, NSPasteboard.NSFilenamesType) > -1)
            {
                string[] files =
                    NSArray.StringArrayFromHandle(pboard.GetPropertyListForType(NSPasteboard.NSFilenamesType).Handle);

                var jsonFile = files.FirstOrDefault(
                    (p => Path.GetExtension(p)
                     .Equals(".JSON", StringComparison.InvariantCultureIgnoreCase))
                );

                if (jsonFile != null)
                {
                    this.OpenAnimationFile(jsonFile);
                }
            }

            return true;
        }

        public void PlayAnimation()
        {
            if (this.lottieLogo.IsAnimationPlaying)
            {
                this.lottieLogo.Pause();
            }
            else
            {
                this.lottieLogo.Play();
            }
        }

        public void RewindAnimation()
        {
            this.lottieLogo.Stop();
        }

        public void ToogleLoop()
        {
            this.lottieLogo.LoopAnimation = !this.lottieLogo.LoopAnimation;
        }

        public nfloat AnimationProgress
        {
            get {
                return this.lottieLogo.AnimationProgress;
            }

            set {
                this.lottieLogo.AnimationProgress = value;
            }
        }

        private void OpenAnimationFile(string file) 
        {
            this.lottieLogo.RemoveFromSuperview();
            this.lottieLogo = null;

            this.lottieLogo = LOTAnimationView.AnimationWithFilePath(file);
			this.lottieLogo.ContentMode = LOTViewContentMode.ScaleAspectFit;
			this.lottieLogo.Frame = this.Bounds;
			this.lottieLogo.AutoresizingMask = NSViewResizingMask.WidthSizable |
				NSViewResizingMask.HeightSizable;

            this.AddSubview(this.lottieLogo, place:NSWindowOrderingMode.Below, otherView:null);
            this.lottieLogo.Play();
		}
    }
}
