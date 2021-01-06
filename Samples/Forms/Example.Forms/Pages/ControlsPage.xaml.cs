using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Example.Forms
{
    public partial class ControlsPage : ContentPage
    {
        public ControlsPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ControlsViewModel controlsViewModel)
            {
                controlsViewModel.AnimationClicked -= ControlsViewModel_AnimationClicked;
                controlsViewModel.AnimationClicked += ControlsViewModel_AnimationClicked;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is ControlsViewModel controlsViewModel)
            {
                controlsViewModel.AnimationClicked -= ControlsViewModel_AnimationClicked;
            }
        }

        private void ControlsViewModel_AnimationClicked(object sender, EventArgs e)
        {
            DisplayAlert("Clicked", "You have clicked on the animation.", "OK");
        }

        private void AnimationView_OnAnimationUpdate(object sender, float e)
        {
            progressSlider.Value = e;
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            animationView.Progress = (float)e.NewValue;
        }
    }
}
