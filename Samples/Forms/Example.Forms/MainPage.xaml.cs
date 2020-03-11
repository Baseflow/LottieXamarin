using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            animationView.Progress = (float)e.NewValue;
        }

        private void Handle_OnFinish(object sender, System.EventArgs e)
        {
            DisplayAlert(string.Empty, $"{nameof(animationView.OnEnded)} invoked!", "OK");
        }

        private void Play_Clicked(object sender, System.EventArgs e)
        {
            animationView.PlayAnimation();
        }

        private void Pause_Clicked(object sender, System.EventArgs e)
        {
            animationView.PauseAnimation();
        }

        private void Resume_Clicked(object sender, System.EventArgs e)
        {
            animationView.ResumeAnimation();
        }

        private void Cancel_Clicked(object sender, System.EventArgs e)
        {
            animationView.CancelAnimation();
        }

        private void Frames_Clicked(object sender, System.EventArgs e)
        {
            animationView.SetMinAndMaxFrame(50, 100);
        }

        private void Segment_Clicked(object sender, System.EventArgs e)
        {
            animationView.SetMinAndMaxProgress(0.65f, 1.0f);
        }
    }
}
