using System.ComponentModel;
using Lottie.Forms;
using Lottie.Forms.Platforms.Wpf;
using LottieSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Platforms.Wpf
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LottieAnimationView>
    {
        private LottieAnimationView _animationView;

        /// <summary>
        ///     Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            // needed because of this linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
#pragma warning disable 0219
            _ = new AnimationViewRenderer();
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
                _animationView.MouseDown -= _animationView_MouseDown;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _animationView = new LottieAnimationView
                    {
                        AutoPlay = false,
                        //PlaybackRate = e.NewElement.Speed,
                        //Scale = new System.Numerics.Vector3(e.NewElement.Scale)
                    };
                    _animationView.Loaded += _animationView_Loaded;
                    _animationView.MouseDown += _animationView_MouseDown;

                    //_animationView.FileName = e.NewElement.Animation as string;
                    _animationView.Composition = e.NewElement.GetAnimation();

                    e.NewElement.PlayCommand = new Command(() =>
                    {
                        _animationView.PlayAnimation();
                        e.NewElement.InvokePlayAnimation();
                    });
                    e.NewElement.PauseCommand = new Command(() =>
                    {
                        _animationView.PauseAnimation();
                        e.NewElement.InvokePauseAnimation();
                    });
                    e.NewElement.ResumeCommand = new Command(() =>
                    {
                        _animationView.ResumeAnimation();
                        e.NewElement.InvokeResumeAnimation();
                    });
                    e.NewElement.StopCommand = new Command(() =>
                    {
                        _animationView.CancelAnimation();
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
                            _animationView.SetMinAndMaxFrame(minFrame, maxFrame);
                            _animationView.PlayAnimation();
                        }
                    });
                    e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        if (paramter is (float minProgress, float maxProgress))
                        {
                            _animationView.SetMinAndMaxProgress(minProgress, maxProgress);
                            _animationView.PlayAnimation();
                        }
                    });
                    e.NewElement.ReverseAnimationSpeedCommand = new Command(() =>
                    {
                        _animationView.ReverseAnimationSpeed();
                    });

                    //e.NewElement.Duration = _animationView.Duration.Ticks;
                    //e.NewElement.IsAnimating = _animationView.IsPlaying;

                    SetNativeControl(_animationView);

                    if (e.NewElement.AutoPlay || e.NewElement.IsAnimating)
                        _animationView.PlayAnimation();
                }
            }
        }

        private void _animationView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Element?.InvokeClick();
        }

        private void _animationView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (Element.AutoPlay || Element.IsAnimating)
            {
                _animationView.PlayAnimation();

                //await Task.Delay(PlayDelay);
                //_ = _animationView.PlayAsync(0, 1, Element.RepeatMode == RepeatMode.Infinite).AsTask();

                //Element.IsAnimating = _animationView.IsPlaying;
                Element.InvokePlayAnimation();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.Composition = Element.GetAnimation();

                if (Element.AutoPlay || Element.IsAnimating)
                    _animationView.PlayAnimation();
            }

            if (e.PropertyName == AnimationView.AutoPlayProperty.PropertyName)
                _animationView.AutoPlay = Element.AutoPlay;

            //if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
            //    _animationView.DefaultCacheStrategy = LottieAnimationView.CacheStrategy.Strong (Element.CacheComposition);

            //if (e.PropertyName == AnimationView.FallbackResource.PropertyName)
            //    _animationView.SetFallbackResource(e.NewElement.FallbackResource);

            //if (e.PropertyName == AnimationView.Composition.PropertyName)
            //    _animationView.Composition = e.NewElement.Composition;

            if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
                _animationView.MinFrame = Element.MinFrame;

            if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
                _animationView.MinProgress = Element.MinProgress;

            if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
                _animationView.MaxFrame = Element.MaxFrame;

            if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
                _animationView.MaxProgress = Element.MaxProgress;

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.Speed = Element.Speed;

            if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName)
                _animationView.RepeatMode = (LottieSharp.RepeatMode)(int)Element.RepeatMode;

            if (e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
                _animationView.RepeatCount = Element.RepeatCount;

            //if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            //    _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
                _animationView.Scale = Element.Scale;

            if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
                _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.Progress = Element.Progress;

            base.OnElementPropertyChanged(sender, e);
        }
    }
}
