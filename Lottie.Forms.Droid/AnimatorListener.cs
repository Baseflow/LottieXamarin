using System;
using Android.Animation;

namespace Lottie.Forms.Droid
{
    public class AnimatorListener : AnimatorListenerAdapter
    {
        private Action _playbackFinished;

        public AnimatorListener(Action playbackFinished)
        {
            _playbackFinished = playbackFinished;
        }

        public override void OnAnimationEnd(Animator animation)
        {
            base.OnAnimationEnd(animation);

            _playbackFinished?.Invoke();
        }

        // TODO support notifying end while looping through below
        //public override void OnAnimationRepeat(Animator animation)
        //{
        //    base.OnAnimationRepeat(animation);
        //}
    }
}
