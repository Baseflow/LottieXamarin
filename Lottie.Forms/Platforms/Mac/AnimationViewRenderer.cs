
//Disable Mac for Xamarin.Forms until Nuget packages are fixed by Xamarin
/*
[assembly: ExportRenderer(typeof(AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.iOS.Renderers
{
    public class AnimationViewRenderer : ViewRenderer<AnimationView, LOTAnimationView>
    {
        private LOTAnimationView _animationView;

        /// <summary>
		///   Used for registration with dependency service
		/// </summary>
		public static new void Init()
        {
            // needed because of this linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
            #pragma warning disable 0219
            var dummy = new AnimationViewRenderer();
            #pragma warning restore 0219
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.OnPlay -= OnPlay;
                e.OldElement.OnPause -= OnPause;
            }

            if (e.NewElement == null) return;

            e.NewElement.OnPlay += OnPlay;
            e.NewElement.OnPause += OnPause;

            if (!string.IsNullOrEmpty(e.NewElement.Animation))
            {
                _animationView = new LOTAnimationView(NSUrl.FromFilename(e.NewElement.Animation))
                {
                    AutoresizingMask = UIViewAutoresizing.All,
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    LoopAnimation = e.NewElement.Loop
                };
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);
                if (e.NewElement.AutoPlay)
                {
                    _animationView.Play();
                }
            }

            if (_animationView != null)
            {
                SetNativeControl(_animationView);
                SetNeedsLayout();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            _animationView?.Play();
            Element.IsPlaying = true;
        }

        private void OnPause(object sender, EventArgs e)
        {
            _animationView?.Pause();
            Element.IsPlaying = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
            {
                _animationView?.RemoveFromSuperview();
                _animationView = new LOTAnimationView(NSUrl.FromString(Element.Animation));
                Element.Duration = TimeSpan.FromMilliseconds(_animationView.AnimationDuration);
                SetNativeControl(_animationView);
                SetNeedsLayout();
                if (Element.AutoPlay)
                {
                    _animationView.Play();
                }
            }

            if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            {
                if (_animationView != null)
                {
                    _animationView.AnimationProgress = Element.Progress;
                }
            }

            if (e.PropertyName == AnimationView.LoopProperty.PropertyName)
            {
                if (_animationView != null)
                {
                    _animationView.LoopAnimation = Element.Loop;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }
    }
}
*/