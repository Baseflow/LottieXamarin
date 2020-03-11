using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.Runtime;

namespace Lottie.Forms.Platforms.Android
{
    public class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
    {
        public AnimatorUpdateListener()
        {
        }

        public AnimatorUpdateListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public Action OnAnimationUpdateImpl { get; set; }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            OnAnimationUpdateImpl?.Invoke();
        }
    }
}
