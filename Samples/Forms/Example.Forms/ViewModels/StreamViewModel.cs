using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Example.Forms
{
    public class StreamViewModel : BaseViewModel
    {
        public StreamViewModel()
        {
            PlayCommand = new Command<AnimationView>((animationView) => {
                animationView.PlayAnimation();
            });
            PauseCommand = new Command<AnimationView>((animationView) => {
                animationView.PauseAnimation();
            });
            CancelCommand = new Command<AnimationView>((animationView) => {
                animationView.CancelAnimation();
            });
            ResumeCommand = new Command<AnimationView>((animationView) => {
                animationView.ResumeAnimation();
            });
            ClickCommand = new Command<AnimationView>((animationView) => {
                //TODO: Show message it is clicked
            });
        }

        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get => _playCommand;
            set => Set(ref _playCommand, value);
        }

        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get => _pauseCommand;
            set => Set(ref _pauseCommand, value);
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set => Set(ref _cancelCommand, value);
        }

        private ICommand _resumeCommand;
        public ICommand ResumeCommand
        {
            get => _resumeCommand;
            set => Set(ref _resumeCommand, value);
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get => _clickCommand;
            set => Set(ref _clickCommand, value);
        }

        private Stream _lottieStream;
        public Stream LottieStream
        {
            get => _lottieStream;
            set => Set(ref _lottieStream, value);
        }
    }
}
