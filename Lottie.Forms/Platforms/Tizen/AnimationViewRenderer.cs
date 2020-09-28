using System.ComponentModel;
using ElottieSharp;
using Lottie.Forms;
using Lottie.Forms.Platforms.Tizen;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]
namespace Lottie.Forms.Platforms.Tizen
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LottieAnimationView>
    {
        private LottieAnimationView _animationView;

        /// <summary>
        /// Used for registration with dependency service
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
                _animationView.Finished -= _animationView_Finished;
                _animationView.Paused -= _animationView_Paused;
                _animationView.Shown -= _animationView_Shown;
                _animationView.Started -= _animationView_Started;
                _animationView.Stopped -= _animationView_Stopped;
                _animationView.FrameUpdated -= _animationView_FrameUpdated;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _animationView = new LottieAnimationView(Xamarin.Forms.Forms.NativeParent);
                    _animationView.Finished += _animationView_Finished;
                    _animationView.Paused += _animationView_Paused;
                    _animationView.Shown += _animationView_Shown;
                    _animationView.Started += _animationView_Started;
                    _animationView.Stopped += _animationView_Stopped;
                    _animationView.FrameUpdated += _animationView_FrameUpdated;

                    /*
                    _clickListener = new ClickListener
                    {
                        OnClickImpl = () => e.NewElement.InvokeClick()
                    };*/

                    _animationView.TrySetAnimation(e.NewElement);

                    e.NewElement.PlayCommand = new Command(() => _animationView.Play());
                    e.NewElement.PauseCommand = new Command(() => _animationView.Pause());
                    e.NewElement.ResumeCommand = new Command(() => _animationView.Play());
                    e.NewElement.StopCommand = new Command(() => _animationView.Stop());
                    //e.NewElement.ClickCommand = new Command(() => _animationView.PerformClick());

                    e.NewElement.PlayMinAndMaxFrameCommand = new Command((object paramter) =>
                    {
                        if (paramter is (int minFrame, int maxFrame))
                        {
                            _animationView.Play(minFrame, maxFrame);
                        }
                    });
                    e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        if (paramter is (float minProgress, float maxProgress))
                        {
                            _animationView.Play(minProgress, maxProgress);
                        }
                    });
                    //e.NewElement.ReverseAnimationSpeedCommand = new Command(() => _animationView.ReverseAnimationSpeed());

                    //_animationView.SetCacheComposition(e.NewElement.CacheComposition);
                    //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
                    //_animationView.Composition = e.NewElement.Composition;

                    //if (e.NewElement.MinFrame != int.MinValue)
                    //    _animationView.SetMinFrame(e.NewElement.MinFrame);
                    if (e.NewElement.MinProgress != float.MinValue)
                        _animationView.MinimumProgress = e.NewElement.MinProgress;
                    //if (e.NewElement.MaxFrame != int.MinValue)
                    //    _animationView.SetMaxFrame(e.NewElement.MaxFrame);
                    if (e.NewElement.MaxProgress != float.MinValue)
                        _animationView.MaximumProgress = e.NewElement.MaxProgress;

                    _animationView.Speed = e.NewElement.Speed;
                    _animationView.AutoRepeat = e.NewElement.RepeatMode == RepeatMode.Infinite;
                    //_animationView.RepeatCount = e.NewElement.RepeatCount;

                    //if (!string.IsNullOrEmpty(e.NewElement.ImageAssetsFolder))
                    //    _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;
                    //_animationView.Scale = e.NewElement.Scale;
                    //_animationView.Frame = e.NewElement.Frame;
                    _animationView.SeekTo(Element.Progress);

                    SetNativeControl(_animationView);

                    if (e.NewElement.AutoPlay || e.NewElement.IsAnimating)
                        _animationView.Play();

                    //e.NewElement.Duration = _animationView.DurationTime;
                    e.NewElement.IsAnimating = _animationView.IsPlaying;
                }
            }
        }

        private void _animationView_FrameUpdated(object sender, FrameEventArgs e)
        {
            Element?.InvokeAnimationUpdate(e.CurrentFrame);
        }

        private void _animationView_Stopped(object sender, System.EventArgs e)
        {
            Element?.InvokeStopAnimation();
        }

        private void _animationView_Started(object sender, System.EventArgs e)
        {
            Element?.InvokePlayAnimation();
        }

        private void _animationView_Shown(object sender, System.EventArgs e)
        {
            
        }

        private void _animationView_Paused(object sender, System.EventArgs e)
        {
            Element?.InvokePauseAnimation();
        }

        private void _animationView_Finished(object sender, System.EventArgs e)
        {
            Element?.InvokePlaybackEnded();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView.TrySetAnimation(Element);

                if (Element.AutoPlay || Element.IsAnimating)
                    _animationView.Play();
            }

            //if (e.PropertyName == AnimationView.AutoPlayProperty.PropertyName)
            //    _animationView.AutoPlay = (Element.AutoPlay);

            //if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
            //    _animationView.SetCacheComposition(Element.CacheComposition);

            //if (e.PropertyName == AnimationView.FallbackResource.PropertyName)
            //    _animationView.SetFallbackResource(e.NewElement.FallbackResource);

            //if (e.PropertyName == AnimationView.Composition.PropertyName)
            //    _animationView.Composition = e.NewElement.Composition;

            //if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
            //    _animationView.SetMinFrame(Element.MinFrame);

            if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
                _animationView.MinimumProgress = Element.MinProgress;

            //if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
            //    _animationView.SetMaxFrame(Element.MaxFrame);

            if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
                _animationView.MaximumProgress = Element.MaxProgress;

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.Speed = (double)new decimal(Element.Speed);

            if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName)
                _animationView.AutoRepeat = Element.RepeatMode == RepeatMode.Infinite;

            //if (e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
            //    _animationView.RepeatCount = Element.RepeatCount;

            //if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            //    _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            //if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
            //    _animationView.Scale = Element.Scale;

            //if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
            //    _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.SeekTo(Element.Progress);

            base.OnElementPropertyChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_animationView != null)
                {
                    _animationView.Finished -= _animationView_Finished;
                    _animationView.Paused -= _animationView_Paused;
                    _animationView.Shown -= _animationView_Shown;
                    _animationView.Started -= _animationView_Started;
                    _animationView.Stopped -= _animationView_Stopped;
                    _animationView.FrameUpdated -= _animationView_FrameUpdated;
                }
            }

            base.Dispose(disposing);
        }
    }
}
