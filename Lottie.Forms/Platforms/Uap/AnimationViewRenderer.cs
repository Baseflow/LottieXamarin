using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.Platforms.Uap;
using Lottie.Forms.UWP.Renderers;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.UWP.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, AnimatedVisualPlayer>
    {
        private AnimatedVisualPlayer _animationView;
        //private bool _needToReverseAnimationSpeed;
        //private bool _needToResetFrames;

        public static int PlayDelay { get; set; } = 1000;

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

                    _animationView.Source.TrySetAnimation(e.NewElement.Animation, e.NewElement.AnimationType, e.NewElement.ImageAssetsFolder);

                    e.NewElement.PlayCommand = new Command(() => _animationView.PlayAsync(0, 1, Element.RepeatMode == RepeatMode.Infinite).AsTask());
                    e.NewElement.PauseCommand = new Command(() => _animationView.Pause());
                    e.NewElement.ResumeCommand = new Command(() => _animationView.Resume());
                    e.NewElement.CancelCommand = new Command(() => _animationView.Stop());
                    //e.NewElement.ClickCommand = new Command(() => _animationView.Click());

                    e.NewElement.SetMinAndMaxFrameCommand = new Command((object paramter) =>
                    {
                        //if (paramter is (int minFrame, int maxFrame))
                            //_animationView.SetMinAndMaxFrame(minFrame, maxFrame);
                    });
                    e.NewElement.SetMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        //if (paramter is (float minProgress, float maxProgress))
                            //_animationView.SetMinAndMaxProgress(minProgress, maxProgress);
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
            Element.InvokeClick();
        }

        private async void _animationView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Element.AutoPlay || Element.IsAnimating)
            {
                await Task.Delay(PlayDelay);
                _ = _animationView.PlayAsync(0, 1, Element.RepeatMode == RepeatMode.Infinite).AsTask();

                Element.IsAnimating = _animationView.IsPlaying;
                Element.InvokePlayAnimation();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.Source.TrySetAnimation(Element.Animation, Element.AnimationType, Element.ImageAssetsFolder);
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

            //if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
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
        */
    }
}
