using System.Windows.Input;
using Xamarin.Forms;

namespace Example.Forms
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            AssetCommand = new Command(() =>
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new AssetPage());
            });
            ControlsCommand = new Command(() =>
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ControlsPage());
            });
            EmbeddedCommand = new Command(() =>
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new EmbeddedPage());
            });
            StreamCommand = new Command(() =>
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new StreamPage());
            });
            UrlCommand = new Command(() =>
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new UrlPage());
            });
        }

        private ICommand _assetCommand;
        public ICommand AssetCommand
        {
            get => _assetCommand;
            set => Set(ref _assetCommand, value);
        }

        private ICommand _controlsCommand;
        public ICommand ControlsCommand
        {
            get => _controlsCommand;
            set => Set(ref _controlsCommand, value);
        }

        private ICommand _embeddedCommand;
        public ICommand EmbeddedCommand
        {
            get => _embeddedCommand;
            set => Set(ref _embeddedCommand, value);
        }

        private ICommand _streamCommand;
        public ICommand StreamCommand
        {
            get => _streamCommand;
            set => Set(ref _streamCommand, value);
        }

        private ICommand _urlCommand;
        public ICommand UrlCommand
        {
            get => _urlCommand;
            set => Set(ref _urlCommand, value);
        }

    }
}
