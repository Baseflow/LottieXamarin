using Android.Animation;
using Android.Runtime;

namespace Lottie.Forms.Platforms.Android
{
    public class AnimatorListener : AnimatorListenerAdapter
    {
        public AnimatorListener()
        {
        }

        public AnimatorListener(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public Action OnAnimationCancelImpl { get; set; }
        public Action OnAnimationEndImpl { get; set; }
        public Action OnAnimationPauseImpl { get; set; }
        public Action OnAnimationRepeatImpl { get; set; }
        public Action OnAnimationResumeImpl { get; set; }
        public Action OnAnimationStartImpl { get; set; }

        public override void OnAnimationCancel(Animator animation)
        {
            base.OnAnimationCancel(animation);
            OnAnimationCancelImpl?.Invoke();
        }

        public override void OnAnimationEnd(Animator animation)
        {
            base.OnAnimationEnd(animation);
            OnAnimationEndImpl?.Invoke();
        }

        public override void OnAnimationPause(Animator animation)
        {
            base.OnAnimationPause(animation);
            OnAnimationPauseImpl?.Invoke();
        }

        public override void OnAnimationRepeat(Animator animation)
        {
            base.OnAnimationRepeat(animation);
            OnAnimationRepeatImpl?.Invoke();
        }

        public override void OnAnimationResume(Animator animation)
        {
            base.OnAnimationResume(animation);
            OnAnimationResumeImpl?.Invoke();
        }

        public override void OnAnimationStart(Animator animation)
        {
            base.OnAnimationStart(animation);
            OnAnimationStartImpl?.Invoke();
        }
    }
}
