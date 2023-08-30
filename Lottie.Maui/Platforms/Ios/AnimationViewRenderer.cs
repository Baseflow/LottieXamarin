using System.ComponentModel;
using Airbnb.Lottie;
using Foundation;
using Lottie.Forms;
using Lottie.Forms.Platforms.Ios;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using UIKit;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Platforms.Ios
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LOTAnimationView>
    {
        private LOTAnimationCompletionBlock _animationCompletionBlock;
        private LOTAnimationView _animationView;
        private UITapGestureRecognizer _gestureRecognizer;
        private int repeatCount = 1;

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (e == null)
                return;

            if (e.OldElement != null)
            {
                CleanupResources();
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _animationCompletionBlock = new LOTAnimationCompletionBlock(AnimationCompletionBlock);

                    _animationView = new LOTAnimationView()
                    {
                        AutoresizingMask = UIViewAutoresizing.All,
                        ContentMode = UIViewContentMode.ScaleAspectFit,
                        LoopAnimation = e.NewElement.RepeatMode == RepeatMode.Infinite,
                        AnimationSpeed = e.NewElement.Speed,
                        AnimationProgress = e.NewElement.Progress,
                        CacheEnable = e.NewElement.CacheComposition,
                        CompletionBlock = _animationCompletionBlock
                    };

                    _animationView.Layer.MasksToBounds = true;

                    var composition = e.NewElement.GetAnimation();
                    _animationView.SceneModel = composition;
                    e.NewElement.InvokeAnimationLoaded(composition);

                    e.NewElement.PlayCommand = new Command(() =>
                    {
                        _animationView.PlayWithCompletion(AnimationCompletionBlock);
                        e.NewElement.InvokePlayAnimation();
                    });
                    e.NewElement.PauseCommand = new Command(() =>
                    {
                        _animationView.Pause();
                        e.NewElement.InvokePauseAnimation();
                    });
                    e.NewElement.ResumeCommand = new Command(() =>
                    {
                        _animationView.PlayWithCompletion(AnimationCompletionBlock);
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
                            _animationView.PlayFromFrame(NSNumber.FromInt32(minFrame), NSNumber.FromInt32(maxFrame), AnimationCompletionBlock);
                    });
                    e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                    {
                        if (paramter is (float minProgress, float maxProgress))
                            _animationView.PlayFromProgress(minProgress, maxProgress, AnimationCompletionBlock);
                    });
                    e.NewElement.ReverseAnimationSpeedCommand = new Command(() => _animationView.AutoReverseAnimation = !_animationView.AutoReverseAnimation);

                    _animationView.CacheEnable = e.NewElement.CacheComposition;
                    //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
                    //_animationView.Composition = e.NewElement.Composition;

                    //TODO: makes animation stop with current default values
                    //_animationView.SetMinFrame(e.NewElement.MinFrame);
                    //_animationView.SetMinProgress(e.NewElement.MinProgress);
                    //_animationView.SetMaxFrame(e.NewElement.MaxFrame);
                    //_animationView.SetMaxProgress(e.NewElement.MaxProgress);

                    _animationView.AnimationSpeed = e.NewElement.Speed;
                    _animationView.LoopAnimation = e.NewElement.RepeatMode == RepeatMode.Infinite;
                    //_animationView.RepeatCount = e.NewElement.RepeatCount;
                    //if (!string.IsNullOrEmpty(e.NewElement.ImageAssetsFolder))
                    //    _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;

                    //TODO: see if this needs to be enabled
                    //_animationView.ContentScaleFactor = Convert.ToSingle(e.NewElement.Scale);

                    //_animationView.Frame = e.NewElement.Frame;
                    _animationView.AnimationProgress = e.NewElement.Progress;

                    _gestureRecognizer = new UITapGestureRecognizer(e.NewElement.InvokeClick);
                    _animationView.AddGestureRecognizer(_gestureRecognizer);

                    SetNativeControl(_animationView);
                    SetNeedsLayout();

                    if (e.NewElement.AutoPlay || e.NewElement.IsAnimating)
                        _animationView.PlayWithCompletion(AnimationCompletionBlock);

                    //e.NewElement.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_animationView == null || Element == null || e == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                //CleanupResources();
                var composition = Element.GetAnimation();
                _animationView.SceneModel = composition;
                Element.InvokeAnimationLoaded(composition);

                if (Element.AutoPlay || Element.IsAnimating)
                    _animationView.PlayWithCompletion(AnimationCompletionBlock);
            }

            if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
                _animationView.CacheEnable = Element.CacheComposition;

            //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
            //_animationView.Composition = e.NewElement.Composition;

            //if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
            //    _animationView.SetMinFrame(Element.MinFrame);

            //if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
            //    _animationView.SetMinProgress(Element.MinProgress);

            //if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
            //    _animationView.SetMaxFrame(Element.MaxFrame);

            //if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
            //    _animationView.SetMaxProgress(Element.MaxProgress);

            if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
                _animationView.AnimationSpeed = Element.Speed;

            if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName)
                _animationView.LoopAnimation = Element.RepeatMode == RepeatMode.Infinite;

            //if (e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
            //    _animationView.RepeatCount = Element.RepeatCount;

            //if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            //    _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

            //if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
            //    _animationView.Scale = Element.Scale;

            //if (e.PropertyName == AnimationView.FrameProperty.PropertyName)
            //    _animationView.Frame = Element.Frame;

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
                _animationView.AnimationProgress = Element.Progress;

            base.OnElementPropertyChanged(sender, e);
        }

        private void AnimationCompletionBlock(bool animationFinished)
        {
            if (animationFinished)
            {
                if (_animationView == null || Element == null)
                    return;

                Element.InvokeFinishedAnimation();

                // Can be null depending if the user callback is executed very quickly 
                // and disposes the Xamarin.Forms page containing the Lottie view
                if (_animationView == null || Element == null)
                    return;

                if (Element.RepeatMode == RepeatMode.Infinite)
                {
                    Element.InvokeRepeatAnimation();
                    _animationView.PlayWithCompletion(AnimationCompletionBlock);
                }
                else if (Element.RepeatMode == RepeatMode.Restart && repeatCount < Element.RepeatCount)
                {
                    repeatCount++;
                    Element.InvokeRepeatAnimation();
                    _animationView.PlayWithCompletion(AnimationCompletionBlock);
                }
                else if (Element.RepeatMode == RepeatMode.Restart && repeatCount == Element.RepeatCount)
                {
                    repeatCount = 1;
                }
            }
        }

        private void CleanupResources()
        {
            repeatCount = 1;

            if (_gestureRecognizer != null)
            {
                _animationView?.RemoveGestureRecognizer(_gestureRecognizer);
                _gestureRecognizer.Dispose();
                _gestureRecognizer = null;
            }

            if (_animationView != null)
            {
                _animationView.RemoveFromSuperview();
                _animationView.Dispose();
                _animationView = null;
            }
        }
    }
}
