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
        public static LOTComposition GetAnimation(this AnimationView animationView)
        {
            var animation = animationView.Animation;

            LOTComposition composition = null;
            switch (animationView.AnimationSource)
            {
                case AnimationSource.AssetOrBundle:
                    if (animation is string bundleAnimation)
                    {
                        if(!string.IsNullOrEmpty(animationView.ImageAssetsFolder))
                            composition = LOTComposition.AnimationNamed(bundleAnimation, NSBundle.FromPath(animationView.ImageAssetsFolder));
                        else
                            composition = LOTComposition.AnimationNamed(bundleAnimation);
                    }
                    break;
                case AnimationSource.Url:
                    if (animation is string stringAnimation)
                        composition = LOTComposition.AnimationNamed(stringAnimation);
                    break;
                case AnimationSource.Json:
                    //TODO: parse to NSDictionary if json is string
                    //if (animation is string jsonAnimation)
                    //composition = LOTComposition.AnimationFromJSON(jsonAnimation);

                    if (animation is NSDictionary dictAnimation)
                        composition = LOTComposition.AnimationFromJSON(dictAnimation);
                    break;
                case AnimationSource.Stream:
                    composition = animationView.GetAnimation(animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    if (animation is string embeddedAnimation)
                    {
                        var assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
                        var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{embeddedAnimation}");

                        if (stream == null)
                        {
                            return null;
                            //throw new FileNotFoundException("Cannot find file.", embeddedAnimation);
                        }
                        composition = animationView.GetAnimation(stream);
                    }
                    break;
                default:
                    break;
            }
            return composition;
        }
        
        public static LOTComposition GetAnimation(this AnimationView animationView, object animation)
        {
            LOTComposition composition = null;
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
            return composition;
        }
    }
}
