using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {
        public ICommand PlayingCommand { get; private set; }
        public ICommand FinishedCommand { get; private set; }
        public ICommand ClickedCommand { get; private set; }

        public MainPage()
        {
            PlayingCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackStartedCommand)} executed!"));

            FinishedCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackFinishedCommand)} executed!"));

            ClickedCommand = new Command(_ =>
                 DisplayAlert($"{nameof(animationView.ClickedCommand)} executed!"));
            
            InitializeComponent();

            playButton.Clicked += (sender, e) => animationView.Play();
            playSegmentsButton.Clicked += (sender, e) => animationView.PlayProgressSegment(0.65f, 0.0f);
            playFramesButton.Clicked += (sender, e) => animationView.PlayFrameSegment(50, 1);
            pauseButton.Clicked += (sender, e) => animationView.Pause();

            BindingContext = this;

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            animationView.Progress = (float) e.NewValue;
        }

        private void Handle_OnFinish(object sender, System.EventArgs e)
        {
            DisplayAlert($"{nameof(animationView.OnFinish)} invoked!");
        }

        private void DisplayAlert(string message)
        {
            DisplayAlert(string.Empty, message, "OK");
        }
    }
}
