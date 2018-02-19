﻿using System;
using System.ComponentModel;
using Lottie.Forms;
using Lottie.Forms.UWP.Renderers;
using LottieUWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.UWP.Renderers
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
            var dummy = new AnimationViewRenderer();
#pragma warning restore 0219
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _animationView = new LottieAnimationView();
                _animationView.AnimatorUpdate += PlaybackFinishedIfProgressReachesOne;
                SetNativeControl(_animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                _animationView.Loop = e.NewElement.Loop;
                _animationView.Speed = e.NewElement.Speed;
                if (!string.IsNullOrEmpty(e.NewElement.Animation))
                {
                    await _animationView.SetAnimationAsync(e.NewElement.Animation);
                    Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);
                }
                if (e.NewElement.AutoPlay)
                    _animationView.PlayAnimation();
            }
        }

        private void PlaybackFinishedIfProgressReachesOne(object sender, ValueAnimator.ValueAnimatorUpdateEventArgs e)
        {
            if (e.Animation.Progress >= 1)
            {
                Element.PlaybackFinished();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            if (_animationView != null)
            {
                _animationView.PlayAnimation();
                Element.IsPlaying = true;
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
            if (_animationView == null)
                return;

            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                await _animationView.SetAnimationAsync(Element.Animation);
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.Duration);
                if (Element.AutoPlay)
                {
                    _animationView.PlayAnimation();
                }
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                _animationView.PauseAnimation();
                _animationView.Progress = Element.Progress;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                _animationView.Loop = Element.Loop;
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                _animationView.Speed = Element.Speed;
            }

            base.OnElementPropertyChanged(sender, e);
        }
    }
}