using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.UWP.Renderers;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Microsoft.UI.Xaml.Controls;
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

        protected override async void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                _animationView = new AnimatedVisualPlayer();

                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;

                _animationView.Tapped -= AnimationViewTapped;
                _animationView = null;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
                e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;

                //_animationView.RepeatCount = e.NewElement.Loop ? -1 : 0;
                //_animationView.Speed = e.NewElement.Speed;
                _animationView.Tapped += AnimationViewTapped;

                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    SetAnimation(e.NewElement.Animation);
                    Element.Duration = _animationView.Duration;
                }

#pragma warning disable CS0618 // Type or member is obsolete
                if(e.NewElement.AutoPlay || e.NewElement.IsPlaying)
#pragma warning restore CS0618 // Type or member is obsolete
                    await _animationView.PlayAsync(0, 1, Element.Loop);
            }
        }

        private void AnimationViewTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Element?.Click();
        }

        private async void OnPlay(object sender, EventArgs e)
        {
            if (_animationView != null)
            {
                if (_animationView.PlaybackRate > 0f)
                {
                    //_animationView.ResumeAnimation();
                }
                else
                {
                    ResetReverse();
                    await _animationView.PlayAsync(0, 1, Element.Loop);
                }
                Element.IsPlaying = true;
            }
        }

        private async Task PrepareReverseAnimation(Action<float, float> action,
                                             float from, float to)
        {
            var minValue = Math.Min(from, to);
            var maxValue = Math.Max(from, to);
            var needReverse = from > to;

            action(minValue, maxValue);

            if (needReverse && !_needToReverseAnimationSpeed)
            {
                _needToReverseAnimationSpeed = true;
                //_animationView.ReverseAnimationSpeed();
            }

            await _animationView.PlayAsync(0, 1, Element.Loop);
            Element.IsPlaying = true;
        }

        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            if (_animationView != null)
            {
                //PrepareReverseAnimation((min, max) =>
                //{
                //    _animationView.SetProgress(min, max);
                //}, e.From, e.To);
            }
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            if (_animationView != null)
            {
                //PrepareReverseAnimation((min, max) =>
                //{
                //    _animationView.SetMinAndMaxFrame((int)min, (int)max);
                //    _needToResetFrames = true;
                //}, e.From, e.To);
            }
        }

        private void OnPause(object sender, EventArgs e)
        {
            if (_animationView != null)
            {
                _animationView.Pause();
                Element.IsPlaying = false;
            }
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName && !string.IsNullOrEmpty(Element.Animation))
            {
                SetAnimation(Element.Animation);
                Element.Duration = _animationView.Duration;

#pragma warning disable CS0618 // Type or member is obsolete
                if (Element.AutoPlay || Element.IsPlaying)
#pragma warning restore CS0618 // Type or member is obsolete
                    await _animationView.PlayAsync(0, 1, Element.Loop);
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.Pause();
                _animationView.SetProgress(Element.Progress);
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                //_animationView.RepeatCount = Element.Loop ? -1 : 0;
            }

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            {
                //_animationView.Speed = Element.Speed;
            }

            if (e.PropertyName == AnimationView.IsPlayingProperty.PropertyName &&
                !string.IsNullOrEmpty(Element.Animation))
            {
                if (Element.IsPlaying)
                    await _animationView.PlayAsync(0,1,Element.Loop);
                else
                    _animationView.Pause();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void ResetReverse()
        {
            //if (_needToResetFrames)
            //{
            //    var composition = _animationView.Composition;

            //    _animationView.SetMinAndMaxFrame((int)composition.StartFrame, (int)composition.EndFrame);
            //    _needToResetFrames = false;
            //}

            //if (_needToReverseAnimationSpeed)
            //{
            //    _needToReverseAnimationSpeed = false;
            //    _animationView.ReverseAnimationSpeed();
            //}
        }

        private void SetAnimation(string animation, string imageAssetsFolder = "")
        {
            var path = Path.Combine("ms-appx:///", string.IsNullOrEmpty(imageAssetsFolder) ? Element.ImageAssetsFolder : imageAssetsFolder, animation);

            _animationView.Source = new LottieVisualSource()
            {
                UriSource = new Uri(path)
            };
        }
    }
}
