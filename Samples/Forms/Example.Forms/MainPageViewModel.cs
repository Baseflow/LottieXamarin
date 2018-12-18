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
            StartPlayingCommand = new Command(() => IsPlaying = true);
            StopPlayingCommand = new Command(() => IsPlaying = false);
            PlayingCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.PlaybackStartedCommand)} executed!"));
            FinishedCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.PlaybackFinishedCommand)} executed!"));
            ClickedCommand = new Command(() => DisplayAlert($"{nameof(AnimationView.ClickedCommand)} executed!"));
        }

        void DisplayAlert(string message) => Application.Current.MainPage.DisplayAlert(string.Empty, message, "OK");

        bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }

        public ICommand StartPlayingCommand { get; }
        public ICommand StopPlayingCommand { get; }
        public ICommand PlayingCommand { get; }
        public ICommand FinishedCommand { get; }
        public ICommand ClickedCommand { get; }
    }
}
