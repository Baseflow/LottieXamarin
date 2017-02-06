using Lottie.Forms;
using Xamarin.Forms;

namespace Example.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            AnimationView.Progress = (float) e.NewValue;
        }
    }
}
