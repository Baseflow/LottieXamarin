using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Example.Forms
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            //StopPlayingCommand = new Command(() => IsPlaying = false);
            //PlayingCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.PlaybackStartedCommand)} executed!"));
            //FinishedCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.PlaybackFinishedCommand)} executed!"));
            //ClickedCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.Command)} executed!"));
        }

        private void DisplayAlert(string message) => Application.Current.MainPage.DisplayAlert(string.Empty, message, "OK");

        private bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private bool _isAnimating;
        public bool IsAnimating
        {
            get => _isAnimating;
            set => Set(ref _isAnimating, value);
        }

        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get => _playCommand;
            set => Set(ref _playCommand, value);
        }

        public ICommand PauseCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ResumeCommand { get; set; }
        public ICommand ClickCommand { get; set; }
    }
}
