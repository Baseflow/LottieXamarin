using Android.Runtime;
using Com.Airbnb.Lottie;

namespace Lottie.Forms.Platforms.Android
{
    public class LottieOnCompositionLoadedListener : Java.Lang.Object, ILottieOnCompositionLoadedListener
    {
        public LottieOnCompositionLoadedListener()
        {
        }

        public LottieOnCompositionLoadedListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public Action<object> OnCompositionLoadedImpl { get; set; }

        public void OnCompositionLoaded(LottieComposition p0)
        {
            OnCompositionLoadedImpl?.Invoke(p0);
        }
    }
}
