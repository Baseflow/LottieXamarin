using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace Lottie.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCompatibility()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans_Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans_Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    handlers.AddCompatibilityRenderer(typeof(Lottie.Forms.AnimationView), typeof(Lottie.Forms.Platforms.Android.AnimationViewRenderer));
#endif
#if IOS
                    handlers.AddCompatibilityRenderer(typeof(Lottie.Forms.AnimationView), typeof(Lottie.Forms.Platforms.Ios.AnimationViewRenderer));
#endif
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
