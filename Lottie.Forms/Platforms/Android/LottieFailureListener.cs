using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;
using Com.Airbnb.Lottie;

namespace Lottie.Forms.Platforms.Android
{
    public class LottieFailureListener : Java.Lang.Object, ILottieListener
    {
        public LottieFailureListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public LottieFailureListener()
        {
        }

        public Action<object> OnResultImpl { get; set; }

        public void OnResult(Java.Lang.Object p0)
        {
            OnResultImpl?.Invoke(p0);
        }
    }
}
