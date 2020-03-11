using System.ComponentModel;
using Airbnb.Lottie;
using Foundation;
using Lottie.Forms;
using Lottie.Forms.Platforms.Ios;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Platforms.Ios
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LOTAnimationView>
    {
        private LOTAnimationCompletionBlock _animationCompletionBlock;
        private LOTAnimationView _animationView;
        private UITapGestureRecognizer _gestureRecognizer;

        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public static new void Init()
        {
            // needed because of this linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
#pragma warning disable 0219
            var dummy = new AnimationViewRenderer();
#pragma warning restore 0219
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (e == null)
                return;

            if (e.OldElement != null)
            {
                CleanupResources();
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    InitAnimationViewForElement(e.NewElement);
                }
            }
        }

        /*
        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            _animationView?.PlayFromProgress(e.From, e.To, PlaybackFinishedIfActually);
            Element.IsPlaying = true;
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            _animationView?.PlayFromFrame(e.From, e.To, PlaybackFinishedIfActually);
            Element.IsPlaying = true;
        }

        private void OnPause(object sender, EventArgs e)
        {
            _animationView?.Pause();
            Element.IsPlaying = false;
        }*/

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName && !string.IsNullOrEmpty(Element.Animation))
            {
                CleanupResources();
                InitAnimationViewForElement(Element);
            }

            if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
                _animationView.CacheEnable = Element.CacheComposition;

            //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
            //_animationView.Composition = e.NewElement.Composition;

            //if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
            //    _animationView.SetMinFrame(Element.MinFrame);

            //if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
            //    _animationView.SetMinProgress(Element.MinProgress);

            //if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
            //    _animationView.SetMaxFrame(Element.MaxFrame);

            //if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            //    _animationView.SetMaxProgress(Element.MaxProgress);

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.AnimationSpeed = Element.Speed;

            if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName)
                _animationView.LoopAnimation = Element.RepeatMode == RepeatMode.Infinite;

            //if (e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
            //    _animationView.RepeatCount = Element.RepeatCount;

            //if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            //    _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            //if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
            //    _animationView.Scale = Element.Scale;

            //if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
            //    _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.AnimationProgress = Element.Progress;

            base.OnElementPropertyChanged(sender, e);
        }

        private void InitAnimationViewForElement(AnimationView element)
        {
            _animationCompletionBlock = new LOTAnimationCompletionBlock(AnimationCompletionBlock);
            _animationView = new LOTAnimationView(NSUrl.FromFilename(element.Animation))
            {
                AutoresizingMask = UIViewAutoresizing.All,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                LoopAnimation = element.RepeatMode == RepeatMode.Infinite,
                AnimationSpeed = element.Speed,
                AnimationProgress = element.Progress,
                CacheEnable = element.CacheComposition,
                CompletionBlock = _animationCompletionBlock
            };
            //_animationView.SetAnimationRepeatCount(e.NewElement.RepeatCount);

            element.PlayCommand = new Command(() => _animationView.Play());
            element.PauseCommand = new Command(() => _animationView.Pause());
            element.ResumeCommand = new Command(() => _animationView.Play());
            element.CancelCommand = new Command(() => _animationView.Stop());
            element.SetMinAndMaxFrameCommand = new Command((object paramter) =>
            {
                //if (paramter is (int minFrame, int maxFrame))
                //    _animationView.SetMinAndMaxFrame(minFrame, maxFrame);
            });
            element.SetMinAndMaxProgressCommand = new Command((object paramter) =>
            {
                //if (paramter is (float minProgress, float maxProgress))
                //    _animationView.SetMinAndMaxProgress(minProgress, maxProgress);
            });
            //e.NewElement.ReverseAnimationSpeedCommand = new Command(() => _animationView.ReverseAnimationSpeed());

            _gestureRecognizer = new UITapGestureRecognizer(element.Click);
            _animationView.AddGestureRecognizer(_gestureRecognizer);

            if (element.AutoPlay || element.IsAnimating)
            {
                _animationView.Play();
            }

            //e.NewElement.Duration = _animationView.AnimationDuration;

            SetNativeControl(_animationView);
            SetNeedsLayout();
        }

        private void AnimationCompletionBlock(bool animationFinished)
        {
            if (animationFinished)
            {
                Element?.InvokePlaybackEnded();
            }
        }

        private void CleanupResources()
        {
            if (_gestureRecognizer != null)
            {
                _animationView?.RemoveGestureRecognizer(_gestureRecognizer);
                _gestureRecognizer.Dispose();
                _gestureRecognizer = null;
            }

            if (_animationView != null)
            {
                _animationView.RemoveFromSuperview();
                _animationView.Dispose();
                _animationView = null;
            }
        }
    }
}
