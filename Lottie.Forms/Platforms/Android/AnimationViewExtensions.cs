using System;
using Com.Airbnb.Lottie;
using System.IO;

namespace Lottie.Forms.Platforms.Android
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this LottieAnimationView animationView, object animation)
        {
            switch (animation)
            {
                case int intAnimation:
                    animationView.SetAnimation(intAnimation);
                    break;
                case string stringAnimation:

                    //TODO: check if json
                    //animationView.SetAnimationFromJson(stringAnimation);
                    //TODO: check if url
                    //animationView.SetAnimationFromUrl(stringAnimation);

                    animationView.SetAnimation(stringAnimation);
                    break;
                case Stream streamAnimation:
                    animationView.SetAnimation(streamAnimation, animationView.Id.ToString());
                    break;
                case null:
                    animationView.ClearAnimation();
                    break;
                default:
                    break;
            }
        }
    }
}
