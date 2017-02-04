using System;
using Lottie.Forms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(Lottie.Forms.AnimationView), typeof(AnimationViewRenderer))]

namespace Lottie.Forms.Droid.Renderers
{
	public class AnimationViewRenderer : ViewRenderer<Lottie.Forms.AnimationView, Com.Airbnb.Lottie.LottieAnimationView>
	{
		public AnimationViewRenderer()
		{
		}

		Com.Airbnb.Lottie.LottieAnimationView animationView;

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<AnimationView> e)
		{
			base.OnElementChanged(e);

            if (Control == null)
			{
				animationView = new Com.Airbnb.Lottie.LottieAnimationView(Context);
				SetNativeControl(animationView);
			}

			if (e.OldElement != null)
			{
				e.OldElement.OnPlay -= this.OnPlay;
				e.OldElement.OnPause -= this.OnPause;
			}

			if (e.NewElement != null)
			{
				e.NewElement.OnPlay += this.OnPlay;
				e.NewElement.OnPause += this.OnPause;
			}
		}

		void OnPlay(object sender, EventArgs e)
		{
			if (animationView != null 
			    && animationView.Handle != IntPtr.Zero)
			{
				animationView.PlayAnimation();
			}
		}

		void OnPause(object sender, EventArgs e)
		{
			if (animationView != null
				&& animationView.Handle != IntPtr.Zero)
			{
				animationView.PauseAnimation();
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
			{
				animationView.SetAnimation(Element.Animation);
			}

			if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
			{
				animationView.Progress = Element.Progress;
			}
			
			base.OnElementPropertyChanged(sender, e);
		}
	}
}
