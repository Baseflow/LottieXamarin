using System;
using ElottieSharp;
using Lottie.Forms;
using Lottie.Forms.EventArguments;
using Lottie.Forms.Tizen;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]
namespace Lottie.Forms.Tizen
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LottieAnimationView>
    {
        private bool _isReady;

        public AnimationViewRenderer()
        {
            /*RegisterPropertyHandler(AnimationView.ProgressProperty, UpdateProgress);
            RegisterPropertyHandler(AnimationView.LoopProperty, UpdateLoop);
            RegisterPropertyHandler(AnimationView.SpeedProperty, UpdateSpeed);
            RegisterPropertyHandler(AnimationView.AnimationProperty, UpdateAnimation);
            RegisterPropertyHandler(AnimationView.IsPlayingProperty, UpdateIsPlaying);*/
        }

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            // needed because of this linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
#pragma warning disable 0219
            var dummy = new AnimationViewRenderer();
#pragma warning restore 0219
        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Element != null)
                {
                    Element.OnPlay -= OnPlay;
                    Element.OnPause -= OnPause;
                    Element.OnPlayProgressSegment -= OnPlayProgressSegment;
                    Element.OnPlayFrameSegment -= OnPlayFrameSegment;
                }

                if (Control != null)
                {
                    Control.Finished -= OnFinished;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            if (Control == null)
            {
                var animationView = new LottieAnimationView(Xamarin.Forms.Forms.NativeParent);
                animationView.Finished += OnFinished;
                SetNativeControl(animationView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
                e.OldElement.OnPlayProgressSegment -= OnPlayProgressSegment;
                e.OldElement.OnPlayFrameSegment -= OnPlayFrameSegment;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnPlay += OnPlay;
                e.NewElement.OnPause += OnPause;
                e.NewElement.OnPlayProgressSegment += OnPlayProgressSegment;
                e.NewElement.OnPlayFrameSegment += OnPlayFrameSegment;
            }
            base.OnElementChanged(e);
        }

        private void OnPlay(object sender, EventArgs e)
        {
            Control?.Play();
            Element.IsPlaying = true;
        }

        private void OnPlayProgressSegment(object sender, ProgressSegmentEventArgs e)
        {
            Control?.Play(e.From, e.To);
            Element.IsPlaying = true;
        }

        private void OnPlayFrameSegment(object sender, FrameSegmentEventArgs e)
        {
            Control?.Play(e.From, e.To);
            Element.IsPlaying = true;
        }

        private void OnPause(object sender, EventArgs e)
        {
            Control?.Pause();
            Element.IsPlaying = false;
        }

        private void OnFinished(object sender, EventArgs e)
        {
            Element?.PlaybackFinished();
        }

        private void UpdateIsPlaying()
        {
            if (!_isReady)
                return;

            if (Element.IsPlaying)
            {
                Control?.Play();
            }
            else
            {
                Control?.Pause();
            }
        }

        private void UpdateAnimation()
        {
            if (!string.IsNullOrEmpty(Element.Animation))
            {
                Control.SetAnimation(ResourcePath.GetPath(Element.Animation));
                Element.Duration = TimeSpan.FromMilliseconds(Control.DurationTime);
                _isReady = true;
            }
            else
            {
                _isReady = false;
            }
        }

        private void UpdateProgress(bool isInitialize)
        {
            if (!isInitialize)
            {
                Control.SeekTo(Element.Progress);
            }
        }

        private void UpdateLoop()
        {
            Control.AutoRepeat = Element.Loop;
        }

        private void UpdateSpeed()
        {
            Control.Speed = (double)new decimal(Element.Speed);
        }*/

    }
}
