using System.IO;
using ElottieSharp;
using Xamarin.Forms.Platform.Tizen;

namespace Lottie.Forms.Platforms.Tizen
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this LottieAnimationView lottieAnimationView, AnimationView animationView)
        {
            if (lottieAnimationView == null)
                throw new ArgumentNullException(nameof(lottieAnimationView));

            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

            switch (animationView.AnimationSource)
            {
                case AnimationSource.AssetOrBundle:
                    lottieAnimationView.TrySetAnimation(animationView, ResourcePath.GetPath(animationView.Animation as string));
                    break;
                case AnimationSource.Url:
                    if (animationView.Animation is string stringAnimation)
                        lottieAnimationView.SetAnimation(stringAnimation);
                    break;
                case AnimationSource.Json:
                    if (animationView.Animation is string jsonAnimation)
                        lottieAnimationView.SetAnimation(jsonAnimation);
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
            if (lottieAnimationView == null)
                throw new ArgumentNullException(nameof(lottieAnimationView));

            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

            switch (animation)
            {
                case int intAnimation:
                    //lottieAnimationView.SetAnimation(intAnimation);
                    break;
                case string stringAnimation:

                    //TODO: check if json
                    //animationView.SetAnimationFromJson(stringAnimation);
                    //TODO: check if url
                    //animationView.SetAnimationFromUrl(stringAnimation);

                    lottieAnimationView.SetAnimation(stringAnimation);
                    break;
                case Stream streamAnimation:
                    //lottieAnimationView.SetAnimation(streamAnimation, null);
                    break;
                case null:
                    lottieAnimationView.Stop();
                    break;
                default:
                    break;
            }
        }
    }
}
