using Xamarin.Forms;

namespace Lottie.Forms
{
	public partial class LottieFormsPage : ContentPage
	{
		readonly string[] Animations = new string[]
		{
			"HamburgerArrow.json",
			"LottieLogo1.json",
			"LottieLogo2.json",
			"PinJump.json",
			"TwitterHeart.json",
		};

		public LottieFormsPage()
		{
			InitializeComponent();

			foreach (var a in Animations)
			{
				animationPicker.Items.Add(a);
			}
		}

		void OnPlay(object sender, System.EventArgs args)
		{
			animationView.Play();
		}

		void OnPause(object sender, System.EventArgs args)
		{
			animationView.Pause();
		}

		void OnAnimationSelected(object sender, System.EventArgs args)
		{
			var animation = Animations[animationPicker.SelectedIndex];

			animationView.Pause();
			animationView.Progress = 0;

			((ViewModels.LottieFormsViewModel)BindingContext).AnimationResource = animation;
		}
	}
}
