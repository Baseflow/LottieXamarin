using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Lottie;
using Foundation;

namespace Lottie.Forms.Platforms.Ios
{
    public static class AnimationViewExtensions
    {
        public static LOTComposition TrySetAnimation(object animation, AnimationType animationType)
        {
            LOTComposition composition = null;
            switch (animationType)
            {
                case AnimationType.AssetOrBundle:
                    //TODO: get the bundle based on ImageAssetsFolderProperty
                    if (animation is string bundleAnimation)
                        composition = LOTComposition.AnimationNamed(bundleAnimation);
                    break;
                case AnimationType.Url:
                    if (animation is string stringAnimation)
                        composition = LOTComposition.AnimationNamed(stringAnimation);
                    break;
                case AnimationType.Json:
                    //if (animation is string jsonAnimation)
                        //composition = LOTComposition.AnimationFromJSON(jsonAnimation);
                    break;
                case AnimationType.Stream:
                    composition.TrySetAnimation(animation);
                    break;
                case AnimationType.EmbeddedResource:
                    if (animation is string embeddedAnimation)
                    {
                        var assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
                        var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{embeddedAnimation}");

                        if (stream == null)
                        {
                            return null;
                            //throw new FileNotFoundException("Cannot find file.", embeddedAnimation);
                        }
                        composition.TrySetAnimation(stream);
                    }
                    break;
                default:
                    break;
            }
            return composition;
        }

        public static void TrySetAnimation(this LOTComposition composition, object animation)
        {
            switch (animation)
            {
                case int intAnimation:
                    //composition = LOTComposition.AnimationNamed(intAnimation);
                    break;
                case string stringAnimation:

                    //TODO: check if json
                    //animationView.SetAnimationFromJson(stringAnimation);
                    //TODO: check if url
                    //animationView.SetAnimationFromUrl(stringAnimation);

                    composition = LOTComposition.AnimationNamed(stringAnimation);
                    break;
                case Stream streamAnimation:
                    //composition = LOTComposition.AnimationNamed(streamAnimation);
                    break;
                case null:
                    composition = null;
                    break;
                default:
                    break;
            }
        }
    }
}
