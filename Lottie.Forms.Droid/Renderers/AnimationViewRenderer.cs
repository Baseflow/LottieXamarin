using System;
using System.ComponentModel;
using Com.Airbnb.Lottie;
using Lottie.Forms;
using Lottie.Forms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Droid.Renderers
{
    public class AnimationViewRenderer :
        Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<AnimationView, LottieAnimationView>
    {
        private LottieAnimationView _animationView;

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _animationView = new LottieAnimationView(Context);
                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            if ((_animationView != null)
                && (_animationView.Handle != IntPtr.Zero))
            {
                _animationView.PlayAnimation();
                Element.IsPlaying = true;
            }
        }

        private void OnPause(object sender, EventArgs e)
        {
            if ((_animationView != null)
                && (_animationView.Handle != IntPtr.Zero))
            {
                _animationView.PauseAnimation();
                Element.IsPlaying = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.SetAnimation(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.Progress = Element.Progress;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                _animationView?.Loop(Element.Loop);
            }

            base.OnElementPropertyChanged(sender, e);
        }
    }
}