using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {            
            InitializeComponent();
            
            playSegmentsButton.Clicked += (sender, e) => animationView.PlayProgressSegment(0.65f, 0.0f);
            playFramesButton.Clicked += (sender, e) => animationView.PlayFrameSegment(50, 1);

            BindingContext = this;

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
