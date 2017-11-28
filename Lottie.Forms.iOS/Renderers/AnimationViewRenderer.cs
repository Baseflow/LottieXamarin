using System;
using System.ComponentModel;
using Airbnb.Lottie;
using Foundation;
using Lottie.Forms;
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
            }

            if (e.NewElement == null) return;

            e.NewElement.OnPlay += OnPlay;
            e.NewElement.OnPause += OnPause;

            if (!string.IsNullOrEmpty(e.NewElement.Animation))
            {
                InitAnimationViewForElement(e.NewElement);
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            _animationView?.PlayWithCompletion(FireOnEndIfFinished);
            Element.IsPlaying = true;
        }

        private void OnPause(object sender, EventArgs e)
        {
            _animationView?.Pause();
            Element.IsPlaying = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView?.RemoveFromSuperview();
                InitAnimationViewForElement(Element);
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                if (_animationView != null)
                {
                    _animationView.AnimationProgress = Element.Progress;
                }
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                if (_animationView != null)
                {
                    _animationView.LoopAnimation = Element.Loop;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void InitAnimationViewForElement(AnimationView theElement)
        {
            _animationView = new LOTAnimationView(NSUrl.FromFilename(theElement.Animation))
            {
                AutoresizingMask = UIViewAutoresizing.All,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                LoopAnimation = theElement.Loop
            };

            Element.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);

            if (theElement.AutoPlay)
            {
                _animationView.PlayWithCompletion(FireOnEndIfFinished);
            }

            if (_animationView != null)
            {
                SetNativeControl(_animationView);
                SetNeedsLayout();
            }
        }

        void FireOnEndIfFinished(bool animationFinished)
        {
            if (animationFinished)
            {
                Element.FireOnFinish();
            }
        }
    }
}