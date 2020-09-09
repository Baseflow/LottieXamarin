using System.IO;
using Com.Airbnb.Lottie;

namespace Lottie.Forms.Platforms.Android
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this LottieAnimationView lottieAnimationView, AnimationView animationView)
        {
            switch (animationView.AnimationSource)
            {
                case AnimationSource.AssetOrBundle:
                    lottieAnimationView.TrySetAnimation(animationView, animationView.Animation);
                    break;
                case AnimationSource.Url:
                    if (animationView.Animation is string stringAnimation)
                        lottieAnimationView.SetAnimationFromUrl(stringAnimation);
                    break;
                case AnimationSource.Json:
                    if (animationView.Animation is string jsonAnimation)
                        lottieAnimationView.SetAnimationFromJson(jsonAnimation);
                    break;
                case AnimationSource.Stream:
                    lottieAnimationView.TrySetAnimation(animationView, animationView.Animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    lottieAnimationView.TrySetAnimation(animationView, animationView.GetStreamFromAssembly());
                    break;
                default:
                    break;
            }
        }

        public static void TrySetAnimation(this LottieAnimationView lottieAnimationView, AnimationView animationView, object animation)
        {
            switch (animation)
            {
                case int intAnimation:
                    lottieAnimationView.SetAnimation(intAnimation);
                    break;
                case string stringAnimation:

                    //TODO: check if json
                    //animationView.SetAnimationFromJson(stringAnimation);
                    //TODO: check if url
                    //animationView.SetAnimationFromUrl(stringAnimation);

                    lottieAnimationView.SetAnimation(stringAnimation);
                    break;
                case Stream streamAnimation:
                    lottieAnimationView.SetAnimation(streamAnimation, null);
                    break;
                case null:
                    lottieAnimationView.ClearAnimation();
                    break;
                default:
                    break;
            }
        }
    }
}
