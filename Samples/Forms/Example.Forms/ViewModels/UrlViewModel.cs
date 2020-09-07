using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace Example.Forms
{
    public class UrlViewModel : BaseViewModel
    {
        public UrlViewModel()
        {
            PlayCommand = new Command<AnimationView>((animationView) =>
            {
                animationView.PlayAnimation();
            });
            PauseCommand = new Command<AnimationView>((animationView) =>
            {
                animationView.PauseAnimation();
            });
            CancelCommand = new Command<AnimationView>((animationView) =>
            {
                animationView.StopAnimation();
            });
            ResumeCommand = new Command<AnimationView>((animationView) =>
            {
                animationView.ResumeAnimation();
            });
            ClickCommand = new Command<AnimationView>((animationView) =>
            {
                //TODO: Show message it is clicked
            });

            Url = "https://assets10.lottiefiles.com/packages/lf20_2LdLki.json";
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

        private string _url;
        public string Url
        {
            get => _url;
            set => Set(ref _url, value);
        }
    }
}
