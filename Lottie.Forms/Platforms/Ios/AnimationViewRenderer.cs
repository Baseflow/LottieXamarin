using System;
using System.ComponentModel;
using Airbnb.Lottie;
using Foundation;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.iOS.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LOTAnimationView>
    {
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

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;

                CleanupResources();
            }

            if (e.NewElement == null) return;

            e.NewElement.OnPlay += OnPlay;
            e.NewElement.OnPause += OnPause;
            e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
            e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;

            if (!string.IsNullOrWhiteSpace(e.NewElement.Animation))
            {
                InitAnimationViewForElement(e.NewElement);
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            _animationView?.PlayWithCompletion(PlaybackFinishedIfActually);
            Element.IsPlaying = true;
        }

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
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName && !string.IsNullOrWhiteSpace(Element.Animation))
            {
                CleanupResources();
                InitAnimationViewForElement(Element);
            }

            if (_animationView == null)
                return;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.AnimationProgress = Element.Progress;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                _animationView.LoopAnimation = Element.Loop;
            }

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            {
                _animationView.AnimationSpeed = Element.Speed;
            }

            if (e.PropertyName == AnimationView.IsPlayingProperty.PropertyName &&
                !string.IsNullOrEmpty(Element.Animation))
            {
                if (Element.IsPlaying)
                    _animationView.PlayWithCompletion(PlaybackFinishedIfActually);
                else
                    _animationView.Pause();
            }
        }

        private void InitAnimationViewForElement(AnimationView theElement)
        {
            _animationView = new LOTAnimationView(NSUrl.FromFilename(theElement.Animation))
            {
                AutoresizingMask = UIViewAutoresizing.All,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                LoopAnimation = theElement.Loop,
                AnimationSpeed = theElement.Speed
            };

            _gestureRecognizer = new UITapGestureRecognizer(theElement.Click);
            _animationView.AddGestureRecognizer(_gestureRecognizer);

            Element.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);

#pragma warning disable CS0618 // Type or member is obsolete
            if (theElement.AutoPlay || theElement.IsPlaying)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                _animationView.PlayWithCompletion(PlaybackFinishedIfActually);
            }

            SetNativeControl(_animationView);
            SetNeedsLayout();
        }

        private void PlaybackFinishedIfActually(bool animationFinished)
        {
            if (animationFinished)
            {
                Element?.PlaybackFinished();
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
