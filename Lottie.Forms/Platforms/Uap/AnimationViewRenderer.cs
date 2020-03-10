using System;
using System.ComponentModel;
using System.IO;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.UWP.Renderers;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.UWP.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, AnimatedVisualPlayer>
    {
        private AnimatedVisualPlayer _animationView;
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

            if (Control == null && e.NewElement != null)
            {
                _animationView = new AnimatedVisualPlayer { AutoPlay = false };
                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnFinish -= OnFinish;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;

                _animationView.Stop();
                _animationView.Tapped -= AnimationViewTapped;
                RestAnimation();
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                e.NewElement.OnFinish += OnFinish;
                e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
                e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;

                _animationView.PlaybackRate = e.NewElement.Speed;
                _animationView.Tapped += AnimationViewTapped;

                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    SetAnimation(e.NewElement.Animation);
                    Element.Duration = _animationView.Duration;
                }
#pragma warning disable CS0618 // Type or member is obsolete
                if (e.NewElement.AutoPlay || e.NewElement.IsPlaying)
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    _ = _animationView.PlayAsync(0, 1, e.NewElement.Loop).AsTask();
                }
                else
                {
                    _animationView.Stop();
                }
            }
        }

        private void OnFinish(object sender, EventArgs e)
        {
            _animationView?.Stop();
            if (Element != null)
            {
                Element.IsPlaying = false;
            }
        }

        private void AnimationViewTapped(object sender, TappedRoutedEventArgs e)
        {
            Element?.Click();
        }

        private void OnPlay(object sender, EventArgs e)
        {
            if (_animationView != null && Element != null)
            {
                Play();
            }
        }

        private void PrepareReverseAnimation(Action<float, float> action, float from, float to)
        {
            var minValue = Math.Min(from, to);
            var maxValue = Math.Max(from, to);
            var needReverse = from > to;

            action(minValue, maxValue);

            if (needReverse && !_needToReverseAnimationSpeed)
            {
                _needToReverseAnimationSpeed = true;
                _animationView.PlaybackRate = -1;
            }

            Play(minValue, maxValue);
        }

        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            if (_animationView != null && Element != null)
            {
                PrepareReverseAnimation((min, max) => Play(min, max), e.From, e.To);
            }
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            if (_animationView != null && Element != null)
            {
                PrepareReverseAnimation((min, max) =>
                {
                    Play(min, max);
                    _needToResetFrames = true;
                }, e.From, e.To);
            }
        }

        private void OnPause(object sender, EventArgs e)
        {
            if (_animationView != null && Element != null)
            {
                _animationView.Pause();
                Element.IsPlaying = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (_animationView == null || Element == null)
            {

                return;
            }

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                if (string.IsNullOrEmpty(Element.Animation))
                {
                    _animationView.Stop();
                    RestAnimation();
                    Element.Duration = TimeSpan.Zero;
                    Element.IsPlaying = false;
                    return;
                }

                SetAnimation(Element.Animation);
                Element.Duration = _animationView.Duration;

#pragma warning disable CS0618 // Type or member is obsolete
                if (Element.AutoPlay || Element.IsPlaying)
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    Play();
                }
                else
                {
                    _animationView.Stop();
                }
            }
            else if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.Pause();
                _animationView.SetProgress(Element.Progress);
            }
            else if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            {
                _animationView.PlaybackRate = Element.Speed;
            }
            else if (e.PropertyName == AnimationView.IsPlayingProperty.PropertyName && !string.IsNullOrEmpty(Element.Animation))
            {
                if (Element?.IsPlaying == true)
                {
                    Play();
                }
                else
                {
                    _animationView?.Pause();
                }
            }
        }

        private void ResetReverse()
        {
            _animationView.PlaybackRate = _animationView.PlaybackRate * -1;
        }

        private void SetAnimation(string animation, string imageAssetsFolder = "")
        {
            var assets = "Assets";

            if (!string.IsNullOrEmpty(imageAssetsFolder))
            {
                assets = imageAssetsFolder;
            }

            if (!string.IsNullOrEmpty(Element.ImageAssetsFolder))
            {
                assets = Element.ImageAssetsFolder;
            }

            var path = Path.Combine("ms-appx:///", assets, animation);

            _animationView.Source = new LottieVisualSource { UriSource = new Uri(path) };
        }

        private void RestAnimation()
        {
            _animationView.Source = null;
        }

        private void Play(double from = 0, double to = 1)
        {
            if (_animationView == null || Element == null)
            {
                return;
            }

            _ = _animationView.PlayAsync(from, to, Element.Loop).AsTask();

            Element.IsPlaying = true;
        }
    }
}
