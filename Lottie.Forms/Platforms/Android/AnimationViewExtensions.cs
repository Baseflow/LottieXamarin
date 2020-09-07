using System.IO;
using Com.Airbnb.Lottie;

namespace Lottie.Forms.Platforms.Android
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this LottieAnimationView animationView, object animation, AnimationSource animationSource)
        {
            switch (animationSource)
            {
                case AnimationSource.AssetOrBundle:
                    animationView.TrySetAnimation(animation);
                    break;
                case AnimationSource.Url:
                    if (animation is string stringAnimation)
                        animationView.SetAnimationFromUrl(stringAnimation, stringAnimation);
                    break;
                case AnimationSource.Json:
                    if (animation is string jsonAnimation)
                        animationView.SetAnimationFromJson(jsonAnimation, null);
                    break;
                case AnimationSource.Stream:
                    animationView.TrySetAnimation(animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    if (animation is string embeddedAnimation)
                    {
                        var assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
                        var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{embeddedAnimation}");

                        if (stream == null)
                        {
                            return;
                            //throw new FileNotFoundException("Cannot find file.", embeddedAnimation);
                        }
                        animationView.TrySetAnimation(stream);
                    }
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
                    animationView.SetAnimation(streamAnimation, null);
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
