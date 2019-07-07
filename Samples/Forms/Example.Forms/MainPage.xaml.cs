using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {            
            InitializeComponent();
            
            // TODO: Currently, reverse playback is not supported on tizen backend. Will update when it supports.
            playSegmentsButton.Clicked += (sender, e) => animationView.PlayProgressSegment(0.65f, (Device.RuntimePlatform != Device.Tizen) ? 0.0f : 1.0f);
            playFramesButton.Clicked += (sender, e) => animationView.PlayFrameSegment(50, (Device.RuntimePlatform != Device.Tizen) ? 1 : 100);

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            animationView.Progress = (float) e.NewValue;
        }

        void Handle_OnFinish(object sender, System.EventArgs e)
        {
            DisplayAlert(string.Empty, $"{nameof(animationView.OnFinish)} invoked!", "OK");
        }
    }
}
