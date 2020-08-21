using System;
using Com.Airbnb.Lottie;
using System.IO;
using System.Reflection;

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
                        animationView.SetAnimationFromJson(jsonAnimation, null);
                    break;
                case AnimationType.Stream:
                    animationView.TrySetAnimation(animation);
                    break;
                case AnimationType.EmbeddedResource:
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
