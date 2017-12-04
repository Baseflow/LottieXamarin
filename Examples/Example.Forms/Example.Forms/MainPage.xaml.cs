using System.Windows.Input;
using Xamarin.Forms;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {
        private readonly ICommand playingCommand;
        private readonly ICommand finishedCommand;

        public ICommand PlayingCommand => playingCommand;

        public ICommand FinishedCommand => finishedCommand;

        public MainPage()
        {
            playingCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackStartedCommand)} executed!"));

            finishedCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackFinishedCommand)} executed!"));

            InitializeComponent();

            playButton.Clicked += (sender, e) => animationView.Play();
            pauseButton.Clicked += (sender, e) => animationView.Pause();

            BindingContext = this;
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
