using System.Windows.Input;
using Xamarin.Forms;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {
        private readonly ICommand playingCommand;
        private readonly ICommand finishedCommand;
        private readonly ICommand clickedCommand;

        public ICommand PlayingCommand => playingCommand;
        public ICommand FinishedCommand => finishedCommand;
        public ICommand ClickedCommand => clickedCommand;

        public MainPage()
        {
            playingCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackStartedCommand)} executed!"));

            finishedCommand = new Command(_ =>
                DisplayAlert($"{nameof(animationView.PlaybackFinishedCommand)} executed!"));

            clickedCommand = new Command(_ =>
                 DisplayAlert($"{nameof(animationView.ClickedCommand)} executed!"));
            
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
