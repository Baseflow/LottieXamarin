using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Example.Forms
{
    public class ControlsViewModel : BaseViewModel
    {
        public ControlsViewModel()
        {
            PlayCommand = new Command<AnimationView>((animationView) => {
                animationView.PlayAnimation();
            });
            PauseCommand = new Command<AnimationView>((animationView) => {
                animationView.PauseAnimation();
            });
            CancelCommand = new Command<AnimationView>((animationView) => {
                animationView.StopAnimation();
            });
            ResumeCommand = new Command<AnimationView>((animationView) => {
                animationView.ResumeAnimation();
            });
            ClickCommand = new Command<AnimationView>((animationView) => {
                //TODO: Show message it is clicked
            });
            MinAndMaxFrameCommand = new Command<AnimationView>((animationView) => {
                animationView.SetMinAndMaxFrame(50, 100);
            });
            MinAndMaxProgressCommand = new Command<AnimationView>((animationView) => {
                animationView.SetMinAndMaxProgress(0.65f, 1.0f);
            });
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

        private ICommand _minAndMaxFrameCommand;
        public ICommand MinAndMaxFrameCommand
        {
            get => _minAndMaxFrameCommand;
            set => Set(ref _minAndMaxFrameCommand, value);
        }

        private ICommand _minAndMaxProgressCommand;
        public ICommand MinAndMaxProgressCommand
        {
            get => _minAndMaxProgressCommand;
            set => Set(ref _minAndMaxProgressCommand, value);
        }
    }
}
