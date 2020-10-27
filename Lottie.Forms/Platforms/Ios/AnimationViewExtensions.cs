using System;
using System.IO;
using Airbnb.Lottie;
using Foundation;

namespace Lottie.Forms.Platforms.Ios
{
    public static class AnimationViewExtensions
    {
        public static LOTComposition GetAnimation(this AnimationView animationView)
        {
            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

            var animation = animationView.Animation;

            LOTComposition composition = null;
            switch (animationView.AnimationSource)
            {
                case AnimationSource.AssetOrBundle:
                    if (animation is string bundleAnimation)
                    {
                        if (!string.IsNullOrEmpty(animationView.ImageAssetsFolder))
                        {
                            var bundle = NSBundle.FromPath(animationView.ImageAssetsFolder);
                            if (bundle != null)
                                composition = LOTComposition.AnimationNamed(bundleAnimation, bundle);
                        }
                        else
                            composition = LOTComposition.AnimationNamed(bundleAnimation);
                    }
                    break;
                case AnimationSource.Url:
                    if (animation is string stringAnimation)
                        composition = LOTComposition.AnimationNamed(stringAnimation);
                    break;
                case AnimationSource.Json:
                    if (animation is string jsonAnimation)
                    {
                        NSData objectData = NSData.FromString(jsonAnimation);
                        NSDictionary jsonData = (NSDictionary)NSJsonSerialization.Deserialize(objectData, NSJsonReadingOptions.MutableContainers, out _);
                        if (jsonData != null)
                            composition = LOTComposition.AnimationFromJSON(jsonData);
                    }
                    else if (animation is NSDictionary dictAnimation)
                        composition = LOTComposition.AnimationFromJSON(dictAnimation);
                    break;
                case AnimationSource.Stream:
                    composition = animationView.GetAnimation(animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    composition = animationView.GetAnimation(animationView.GetStreamFromAssembly());
                    break;
                default:
                    break;
            }
            return composition;
        }

        public static LOTComposition GetAnimation(this AnimationView animationView, object animation)
        {
            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

            LOTComposition composition = null;
            switch (animation)
            {
                case string stringAnimation:

                    //TODO: check if json
                    //animationView.SetAnimationFromJson(stringAnimation);
                    //TODO: check if url
                    //animationView.SetAnimationFromUrl(stringAnimation);

                    composition = LOTComposition.AnimationNamed(stringAnimation);
                    break;
                case Stream streamAnimation:
                    using (StreamReader reader = new StreamReader(streamAnimation))
                    {
                        string json = reader.ReadToEnd();
                        NSData objectData = NSData.FromString(json);
                        NSDictionary jsonData = (NSDictionary)NSJsonSerialization.Deserialize(objectData, NSJsonReadingOptions.MutableContainers, out _);
                        if (jsonData != null)
                            composition = LOTComposition.AnimationFromJSON(jsonData);
                    }
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
