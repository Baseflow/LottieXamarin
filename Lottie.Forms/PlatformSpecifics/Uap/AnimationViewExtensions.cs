using Xamarin.Forms;
using WindowsSpecific = Xamarin.Forms.PlatformConfiguration.Windows;
using FormsElement = Lottie.Forms.AnimationView;

namespace Lottie.Forms.PlatformConfiguration.UWPSpecific
{
    public static class AnimationViewExtensions
    {
        public static readonly BindableProperty DelayBeforeLoadMillisecondsProperty
            = BindableProperty.Create(nameof(DelayBeforeLoadMilliseconds), typeof(int), typeof(AnimationViewExtensions), 1000);

        public static int GetDelayBeforeLoadMilliseconds(BindableObject element)
            => (int)element?.GetValue(DelayBeforeLoadMillisecondsProperty);

        public static void SetDelayBeforeLoadMilliseconds(BindableObject element, int value)
            => element?.SetValue(DelayBeforeLoadMillisecondsProperty, value);

        public static int DelayBeforeLoadMilliseconds(this IPlatformElementConfiguration<WindowsSpecific, FormsElement> config)
            => GetDelayBeforeLoadMilliseconds(config?.Element);

        public static IPlatformElementConfiguration<WindowsSpecific, FormsElement> SetDelayBeforeLoadMilliseconds(this IPlatformElementConfiguration<WindowsSpecific, FormsElement> config, int value)
        {
            SetDelayBeforeLoadMilliseconds(config?.Element, value);
            return config;
        }
    }
}
