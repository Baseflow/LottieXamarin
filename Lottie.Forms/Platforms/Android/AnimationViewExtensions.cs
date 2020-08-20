using System;
using Com.Airbnb.Lottie;
using System.IO;

namespace Lottie.Forms.Platforms.Android
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this LottieAnimationView animationView, object animation, AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.AssetOrBundle:
                    animationView.TrySetAnimation(animation);
                    break;
                case AnimationType.Url:
                    if(animation is string stringAnimation)
                        animationView.SetAnimationFromUrl(stringAnimation, stringAnimation);
                    break;
                case AnimationType.Json:
                    if (animation is string jsonAnimation)
                        animationView.SetAnimationFromJson(jsonAnimation, animationView.Id.ToString());
                    break;
                case AnimationType.Stream:
                    animationView.TrySetAnimation(animation);
                    break;
                case AnimationType.Embedded:
                    //TODO
                    break;
                default:
                    break;
            }
        }
        
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
