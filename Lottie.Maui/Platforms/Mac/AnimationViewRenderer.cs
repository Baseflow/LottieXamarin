﻿using System.ComponentModel;
using Airbnb.Lottie;
using AppKit;
using Foundation;
using Lottie.Forms;
using Lottie.Forms.Platforms.Mac;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer)), Xamarin.Forms.Internals.Preserve(AllMembers = true)]

namespace Lottie.Forms.Platforms.Mac
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LOTAnimationView>
    {
        private LOTAnimationCompletionBlock _animationCompletionBlock;
        private LOTAnimationView _animationView;
        private NSClickGestureRecognizer _gestureRecognizer;
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
                        ContentMode = LOTViewContentMode.ScaleAspectFill,
                        Frame = Bounds,
                        AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
                        LoopAnimation = e.NewElement.RepeatMode == RepeatMode.Infinite,
                        AnimationSpeed = e.NewElement.Speed,
                        AnimationProgress = e.NewElement.Progress,
                        CacheEnable = e.NewElement.CacheComposition,
                        CompletionBlock = _animationCompletionBlock
                    };

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
                    //_animationView.ContentScaleFactor = e.NewElement.Scale;
                    //_animationView.Frame = e.NewElement.Frame;
                    _animationView.AnimationProgress = e.NewElement.Progress;

                    _gestureRecognizer = new NSClickGestureRecognizer(e.NewElement.InvokeClick);
                    _animationView.AddGestureRecognizer(_gestureRecognizer);

                    SetNativeControl(_animationView);
                    //SetNeedsLayout();

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
                Element?.InvokeFinishedAnimation();
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
