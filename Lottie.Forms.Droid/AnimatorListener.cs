using System;
using Android.Animation;

namespace Lottie.Forms.Droid
{
    public class AnimatorListener : AnimatorListenerAdapter
    {
        private Action _fireOnEnd;

        public AnimatorListener(Action fireOnEnd)
        {
            _fireOnEnd = fireOnEnd;
        }

        public override void OnAnimationEnd(Animator animation)
        {
            base.OnAnimationEnd(animation);

            _fireOnEnd?.Invoke();
        }

        // TODO support notifying end while looping through below
        //public override void OnAnimationRepeat(Animator animation)
        //{
        //    base.OnAnimationRepeat(animation);
        //}
    }
}
