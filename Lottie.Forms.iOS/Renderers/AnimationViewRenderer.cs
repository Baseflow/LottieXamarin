using System;
using System.ComponentModel;
using Airbnb.Lottie;
using Lottie.Forms;
using Lottie.Forms.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.iOS.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LAAnimationView>
    {
        private LAAnimationView _animationView;

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
                _animationView = LAAnimationView.AnimationNamed(e.NewElement.Animation);
            }

            if (_animationView != null)
            {
                AddSubview(_animationView);
                SetNeedsLayout();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            _animationView?.Play();
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
                _animationView = LAAnimationView.AnimationNamed(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);
                AddSubview(_animationView);
                SetNeedsLayout();
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
    }
}