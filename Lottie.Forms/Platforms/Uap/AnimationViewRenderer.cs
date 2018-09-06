using System;
using System.ComponentModel;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.UWP.Renderers;
using LottieUWP;
using LottieUWP.Utils;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.UWP.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LottieAnimationView>
    {
        private LottieAnimationView _animationView;
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

        protected override async void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                _animationView = new LottieAnimationView();

                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;

                _animationView.Tapped -= AnimationViewTapped;

                _animationView.AnimatorUpdate -= PlaybackFinishedIfProgressReachesOne;

                _animationView = null;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
                e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;

                _animationView.RepeatCount = e.NewElement.Loop ? -1 : 0;
                _animationView.Speed = e.NewElement.Speed;
                _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;
                _animationView.Tapped += AnimationViewTapped;
                _animationView.AnimatorUpdate += PlaybackFinishedIfProgressReachesOne;

                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    await _animationView.SetAnimationAsync(e.NewElement.Animation);
                    Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);
                }

                if (e.NewElement.AutoPlay)
                    _animationView.PlayAnimation();
            }
        }

        private void AnimationViewTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Element?.Click();
        }

        private void PlaybackFinishedIfProgressReachesOne(object sender, ValueAnimator.ValueAnimatorUpdateEventArgs e)
        {
            if (((LottieValueAnimator)e.Animation).AnimatedValueAbsolute >= 1)
            {
                Element?.PlaybackFinished();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            if (_animationView != null)
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

        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            if (_animationView != null)
            {
                PrepareReverseAnimation((min, max) =>
                {
                    _animationView.SetMinAndMaxProgress(min, max);
                }, e.From, e.To);
            }
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            if (_animationView != null)
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
            if (_animationView != null)
            {
                _animationView.PauseAnimation();
                Element.IsPlaying = false;
            }
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || Element.Animation == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                await _animationView.SetAnimationAsync(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);

                if (Element.AutoPlay)
                    _animationView.PlayAnimation();
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.PauseAnimation();
                _animationView.Progress = Element.Progress;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                _animationView.RepeatCount = Element.Loop ? -1 : 0;
            }

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            {
                _animationView.Speed = Element.Speed;
            }

            if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName)
            {
                _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;
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
                _needToReverseAnimationSpeed = false;
                _animationView.ReverseAnimationSpeed();
            }
        }
    }
}
