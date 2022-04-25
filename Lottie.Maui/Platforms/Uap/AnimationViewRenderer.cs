using System.ComponentModel;
using Lottie.Forms;
using Lottie.Forms.Platforms.Uap;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer)), Xamarin.Forms.Internals.Preserve(AllMembers = true)]

namespace Lottie.Forms.Platforms.Uap
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, AnimatedVisualPlayer>
    {
        private AnimatedVisualPlayer _animationView;
        //private bool _needToReverseAnimationSpeed;
        //private bool _needToResetFrames;

        public static int PlayDelay { get; set; } = 1000;

        protected override async void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (e == null)
                return;

            if (e.OldElement != null)
            {
                _animationView.Loaded -= _animationView_Loaded;
                _animationView.Tapped -= _animationView_Tapped;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _animationView = new AnimatedVisualPlayer
                    {
                        AutoPlay = false,
                        PlaybackRate = e.NewElement.Speed,
                        //Scale = new System.Numerics.Vector3(e.NewElement.Scale)
                    };
                    _animationView.Loaded += _animationView_Loaded;
                    _animationView.Tapped += _animationView_Tapped;

                    var composition = await e.NewElement.GetAnimationAsync();
                    _animationView.Source = composition;
                    e.NewElement.InvokeAnimationLoaded(composition);

                    e.NewElement.PlayCommand = new Command(() =>
                    {
                        _animationView.PlayAsync(0, 1, Element.RepeatMode == RepeatMode.Infinite).AsTask();
                        e.NewElement.InvokePlayAnimation();
                    });
                    e.NewElement.PauseCommand = new Command(() =>
                    {
                        _animationView.Pause();
                        e.NewElement.InvokePauseAnimation();
                    });
                    e.NewElement.ResumeCommand = new Command(() =>
                    {
                        _animationView.Resume();
                        e.NewElement.InvokeResumeAnimation();
                    });
                    e.NewElement.StopCommand = new Command(() =>
                    {
                        _animationView.Stop();
                        e.NewElement.InvokeStopAnimation();
                    });
                    e.NewElement.ClickCommand = new Command(() =>
                    {
                        //_animationView.Click();
                        //e.NewElement.InvokeClick();
                    });

                    e.NewElement.PlayMinAndMaxFrameCommand = new Command((object paramter) =>
                    {
                        if (paramter is (int minFrame, int maxFrame))
                        {
                            _ = _animationView.PlayAsync(minFrame, maxFrame, Element.RepeatMode == RepeatMode.Infinite).AsTask();
                        }
                    });
                    e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        if (paramter is (float minProgress, float maxProgress))
                        {
                            _ = _animationView.PlayAsync(minProgress, maxProgress, Element.RepeatMode == RepeatMode.Infinite).AsTask();
                        }
                    });
                    e.NewElement.ReverseAnimationSpeedCommand = new Command(() =>
                    {
                        _animationView.PlaybackRate = -1;
                    });

                    e.NewElement.Duration = _animationView.Duration.Ticks;
                    e.NewElement.IsAnimating = _animationView.IsPlaying;

                    SetNativeControl(_animationView);
                }
            }
        }

        private void _animationView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Element?.InvokeClick();
        }

        private async void _animationView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (Element.AutoPlay || Element.IsAnimating)
            {
                await Task.Delay(PlayDelay);
                _ = _animationView.PlayAsync(0, 1, Element.RepeatMode == RepeatMode.Infinite).AsTask();

                Element.IsAnimating = _animationView.IsPlaying;
                Element.InvokePlayAnimation();
            }
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                var composition = await Element.GetAnimationAsync();
                _animationView.Source = composition;
                Element.InvokeAnimationLoaded(composition);
            }

            if (e.PropertyName == AnimationView.AutoPlayProperty.PropertyName)
                _animationView.AutoPlay = Element.AutoPlay;

            //if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
            //    _animationView.SetCacheComposition(Element.CacheComposition);

            //if (e.PropertyName == AnimationView.FallbackResource.PropertyName)
            //    _animationView.SetFallbackResource(e.NewElement.FallbackResource);

            //if (e.PropertyName == AnimationView.Composition.PropertyName)
            //    _animationView.Composition = e.NewElement.Composition;

            //if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
            //    _animationView.SetMinFrame(Element.MinFrame);

            //if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
            //    _animationView.SetMinProgress(Element.MinProgress);

            //if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
            //    _animationView.SetMaxFrame(Element.MaxFrame);

            //if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
            //    _animationView.SetMaxProgress(Element.MaxProgress);

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.PlaybackRate = Element.Speed;

            //if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName)
            //    _animationView.RepeatMode = (int)Element.RepeatMode;

            //if (e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
            //    _animationView.RepeatCount = Element.RepeatCount;

            //if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            //    _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            //if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
            //    _animationView.Scale = Element.Scale;

            //if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
            //    _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.SetProgress(Element.Progress);

            base.OnElementPropertyChanged(sender, e);
        }

        /*
                
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
        */
    }
}
