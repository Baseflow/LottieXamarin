using System;
namespace Lottie.Forms
{
	public class AnimationView : Xamarin.Forms.View
	{
		public static readonly Xamarin.Forms.BindableProperty ProgressProperty = Xamarin.Forms.BindableProperty.Create(nameof(Progress), typeof(float), typeof(Lottie.Forms.AnimationView), default(float));
		public float Progress
		{
			get
			{
				return (float)GetValue(ProgressProperty);
			}

			set
			{
				SetValue(ProgressProperty, value);
			}
		}

		public static readonly Xamarin.Forms.BindableProperty AnimationProperty = Xamarin.Forms.BindableProperty.Create(nameof(Animation), typeof(string), typeof(Lottie.Forms.AnimationView), default(string), Xamarin.Forms.BindingMode.OneWay);
		public string Animation
		{
			get
			{
				return (string)GetValue(AnimationProperty);
			}

			set
			{
				SetValue(AnimationProperty, value);
			}
		}

		// TODO: Loop, Autoplay, IsPlaying, Duration

		public event EventHandler OnPlay;
		public void Play()
		{
			OnPlay?.Invoke(this, new EventArgs());
		}

		public event EventHandler OnPause;
		public void Pause()
		{
			OnPause?.Invoke(this, new EventArgs());
		}
	}
}