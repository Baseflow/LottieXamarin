using System;
using System.ComponentModel;
using Com.Airbnb.Lottie;
using Lottie.Forms;
using Lottie.Forms.Droid;
using Lottie.Forms.EventArguments;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Droid
{
#pragma warning disable 0618
    public class AnimationViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<AnimationView,
        LottieAnimationView>
    {
        private LottieAnimationView _animationView;
        private AnimatorListener _animatorListener;
        private bool _needToReverseAnimationSpeed;
        private bool _needToResetFrames;

        /// <summary>
        ///     Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            // needed because of this linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
#pragma warning disable 0219
            var dummy = new AnimationViewRenderer();
#pragma warning restore 0219
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _animationView = new LottieAnimationView(Context);
                _animatorListener = new AnimatorListener(PlaybackFinished);
                _animationView.AddAnimatorListener(_animatorListener);

                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;

                _animationView.SetOnClickListener(null);
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
                e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;

                _animationView.Speed = e.NewElement.Speed;
                _animationView.Loop(e.NewElement.Loop);
                _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;

                _animationView.SetOnClickListener(new ClickListener(e.NewElement));

                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    _animationView.SetAnimation(e.NewElement.Animation);
                    Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);
                }

                if (e.NewElement.AutoPlay) 
                    _animationView.PlayAnimation();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            if (_animationView != null
                && _animationView.Handle != IntPtr.Zero)
            {
                if (_animationView.Progress > 0f)
                {
                    _animationView.ResumeAnimation();
                }
                else
                {
                    ResetReverse();
                    _animationView.PlayAnimation();
                }
                Element.IsPlaying = true;
            }
        }

        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            if (_animationView != null 
                && _animationView.Handle != IntPtr.Zero)
            {
                PrepareReverseAnimation((min, max) => 
                {
                    _animationView.SetMinAndMaxProgress(min, max);
                }, e.From, e.To);
            }
        }

        private void PrepareReverseAnimation(Action<float, float> action, 
                                             float from, float to)
        {
            var minValue = Math.Min(from, to);
            var maxValue = Math.Max(from, to);
            var needReverse = from > to;

            action(minValue, maxValue);

            if (needReverse && !_needToReverseAnimationSpeed)
            {
                _needToReverseAnimationSpeed = true;
                _animationView.ReverseAnimationSpeed();
            }

            _animationView.PlayAnimation();
            Element.IsPlaying = true;
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            if (_animationView != null
                && _animationView.Handle != IntPtr.Zero)
            {
                PrepareReverseAnimation((min, max) =>
                {
                    _animationView.SetMinAndMaxFrame((int)min, (int)max);
                    _needToResetFrames = true;
                }, e.From, e.To);
            }
        }

        private void OnPause(object sender, EventArgs e)
        {
            if (_animationView != null
                && _animationView.Handle != IntPtr.Zero)
            {
                _animationView.PauseAnimation();
                Element.IsPlaying = false;
            }
        }

        private void PlaybackFinished()
        {
            Element.PlaybackFinished();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			if (_animationView == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.SetAnimation(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);

                if (Element.AutoPlay) 
                    _animationView.PlayAnimation();
            }

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.Speed = Element.Speed;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.PauseAnimation();
                _animationView.Progress = Element.Progress;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName) 
                _animationView.Loop(Element.Loop);

            if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName)
                _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            base.OnElementPropertyChanged(sender, e);
        }

        private void ResetReverse()
        {
            if (_needToResetFrames)
            {
                var composition = _animationView.Composition;

                _animationView.SetMinAndMaxFrame((int)composition.StartFrame, (int)composition.EndFrame);
                _needToResetFrames = false;
            }
                          
            if (_needToReverseAnimationSpeed)
            {
                _animationView.ReverseAnimationSpeed();
                _needToReverseAnimationSpeed = false;
            }
        }

        public class ClickListener : Java.Lang.Object, IOnClickListener
        {
            private readonly AnimationView _animationView;

            public ClickListener(AnimationView animationView)
            {
                _animationView = animationView;
            }

            public void OnClick(Android.Views.View v)
            {
                _animationView.Click();
            }
        }
    }
#pragma warning restore 0618
}