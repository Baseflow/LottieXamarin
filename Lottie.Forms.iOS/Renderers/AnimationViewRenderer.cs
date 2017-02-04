using System;
using Lottie.Forms.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Airbnb.Lottie;

[assembly: ExportRenderer(typeof(Lottie.Forms.AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.iOS.Renderers
{
	public class AnimationViewRenderer : ViewRenderer<Lottie.Forms.AnimationView, LAAnimationView>
	{
		public AnimationViewRenderer()
		{
		}

		UIKit.UIContentContainer container;
		LAAnimationView animationView;

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<AnimationView> e)
		{
			base.OnElementChanged(e);

		//	container = new UIKit.UIContentContainer();

			if (e.OldElement != null)
			{
				e.OldElement.OnPlay -= this.OnPlay;
				e.OldElement.OnPause -= this.OnPause;
			}

			if (e.NewElement != null)
			{
				e.NewElement.OnPlay += this.OnPlay;
				e.NewElement.OnPause += this.OnPause;

				if (!string.IsNullOrEmpty(e.NewElement.Animation))
				{
					animationView = LAAnimationView.AnimationNamed(e.NewElement.Animation);
				}

				if (animationView != null)
				{
					AddSubview(animationView);
					SetNeedsLayout();
				}
			}
		}

		void OnPlay(object sender, EventArgs e)
		{
			if (animationView != null )
			{
				animationView.Play();
			}
		}

		void OnPause(object sender, EventArgs e)
		{
			if (animationView != null)
			{
				animationView.Pause();
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
			{
				if (animationView != null)
				{
					animationView.RemoveFromSuperview();
				}
				animationView = LAAnimationView.AnimationNamed(Element.Animation);

				AddSubview(animationView);
				SetNeedsLayout();
			}

			if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
			{
				if (animationView != null)
				{
					animationView.AnimationProgress = Element.Progress;
				}
			}
			
			base.OnElementPropertyChanged(sender, e);
		}
	}
}
