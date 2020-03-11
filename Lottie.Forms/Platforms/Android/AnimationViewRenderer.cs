using System;
using System.ComponentModel;
using Com.Airbnb.Lottie;
using Lottie.Forms;
using Lottie.Forms.Platforms.Android;
using Lottie.Forms.EventArguments;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]
namespace Lottie.Forms.Platforms.Android
{
#pragma warning disable 0618
    public class AnimationViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<AnimationView,
        LottieAnimationView>
    {
        private LottieAnimationView _animationView;
        private AnimatorListener _animatorListener;
        private AnimatorUpdateListener _animatorUpdateListener;
        private LottieOnCompositionLoadedListener _lottieOnCompositionLoadedListener;
        private LottieFailureListener _lottieFailureListener;
        private ClickListener _clickListener;

        //private bool _needToReverseAnimationSpeed;
        //private bool _needToResetFrames;

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
                _animatorListener = new AnimatorListener();
                _animatorUpdateListener = new AnimatorUpdateListener();
                _lottieOnCompositionLoadedListener = new LottieOnCompositionLoadedListener();
                _lottieFailureListener = new LottieFailureListener();

                _animationView.AddAnimatorListener(_animatorListener);
                _animationView.AddAnimatorUpdateListener(_animatorUpdateListener);
                _animationView.AddLottieOnCompositionLoadedListener(_lottieOnCompositionLoadedListener);
                _animationView.SetFailureListener(_lottieFailureListener);

                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                /*e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;*/

                _animationView.SetOnClickListener(null);
                _animationView.SetFailureListener(null);
            }

            if (e.NewElement != null)
            {
                _animatorListener.OnAnimationCancelImpl = () => e.NewElement.InvokeCancelAnimation();
                _animatorListener.OnAnimationEndImpl = () => e.NewElement.InvokePlaybackEnded();
                _animatorListener.OnAnimationPauseImpl = () => e.NewElement.InvokePauseAnimation();
                _animatorListener.OnAnimationRepeatImpl = () => e.NewElement.InvokeRepeatAnimation();
                _animatorListener.OnAnimationResumeImpl = () => e.NewElement.InvokeResumeAnimation();
                _animatorListener.OnAnimationStartImpl = () => e.NewElement.InvokePlayAnimation();

                _animatorUpdateListener.OnAnimationUpdateImpl = () => e.NewElement.InvokeAnimatorUpdate();

                _lottieOnCompositionLoadedListener.OnCompositionLoadedImpl = (composition) => e.NewElement.InvokeCompositionLoaded(composition);

                _lottieFailureListener.OnResultImpl = (result) => e.NewElement.InvokeFailure(new Exception("Failed"));

                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    _animationView.SetAnimation(e.NewElement.Animation);
                    Element.Duration = _animationView.Duration;
                }

                _clickListener = new ClickListener
                {
                    OnClickImpl = () => e.NewElement.Click()
                };
                _animationView.SetOnClickListener(_clickListener);

                e.NewElement.NativePlayCommand = new Command(() => _animationView.PlayAnimation());
                e.NewElement.NativePauseCommand = new Command(() => _animationView.PauseAnimation());
                e.NewElement.NativeResumeCommand = new Command(() => _animationView.ResumeAnimation());
                e.NewElement.NativeCancelCommand = new Command(() => _animationView.CancelAnimation());

                _animationView.Speed = e.NewElement.Speed;
                _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;

                if (e.NewElement.AutoPlay)
                    _animationView.PlayAnimation();

                //_animationView.SetRenderMode(e.NewElement.HardwareAcceleration ? RenderMode.Hardware : RenderMode.Software);
                //_animationView.Loop(e.NewElement.Loop);
            }
        }

        /*
        private void OnPlayAnimation(object sender, EventArgs e)
        {
            if (_animationView != null && _animationView.Handle != IntPtr.Zero)
            {
                if (_animationView.Progress > 0f)
                {
                    _animationView.ResumeAnimation();
                }
                else
                {
                    _animationView.PlayAnimation();
                }
                Element.IsAnimating = true;
            }
        }

        private void OnPauseAnimation(object sender, EventArgs e)
        {
            if (_animationView != null && _animationView.Handle != IntPtr.Zero)
            {
                _animationView.PauseAnimation();
                Element.IsAnimating = false;
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



        private void PlaybackFinished()
        {
            Element?.PlaybackFinished();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName && !string.IsNullOrEmpty(Element.Animation))
            {
                _animationView.SetAnimation(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);

                if (Element.AutoPlay || Element.IsPlaying)
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

            if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
                _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            if (e.PropertyName == AnimationView.HardwareAccelerationProperty.PropertyName)
                _animationView.SetRenderMode(Element.HardwareAcceleration ? RenderMode.Hardware : RenderMode.Software);

            if (e.PropertyName == AnimationView.IsPlayingProperty.PropertyName &&
                !string.IsNullOrEmpty(Element.Animation))
            {
                if (Element.IsPlaying)
                    _animationView.PlayAnimation();
                else
                    _animationView.PauseAnimation();
            }

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
        }*/
    }
#pragma warning restore 0618
}
