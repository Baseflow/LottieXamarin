using Android.Animation;
using Android.Runtime;

namespace Lottie.Maui.Platforms.Android
{
    public class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
    {
        public AnimatorUpdateListener()
        {
        }

        public AnimatorUpdateListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public Action<float> OnAnimationUpdateImpl { get; set; }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            if (animation == null)
                throw new ArgumentNullException(nameof(animation));

            OnAnimationUpdateImpl?.Invoke(((float)animation.AnimatedValue));
        }
    }
}
