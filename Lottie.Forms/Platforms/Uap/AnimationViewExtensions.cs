using System;
using System.IO;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Lottie;

namespace Lottie.Forms.Platforms.Uap
{
    public static class AnimationViewExtensions
    {
        public static void TrySetAnimation(this IAnimatedVisualSource animatedVisualSource, object animation, AnimationSource animationSource, string imageAssetsFolder = null)
        {
            switch (animationSource)
            {
                case AnimationSource.AssetOrBundle:
                    var assets = "Assets";

                    if (!string.IsNullOrEmpty(imageAssetsFolder))
                    {
                        assets = imageAssetsFolder;
                    }

                    var path = $"ms-appx:///{assets}/{animation}";
                    animatedVisualSource.TrySetAnimation(path);
                    break;
                case AnimationSource.Url:
                    if(animation is string stringAnimation)
                        animatedVisualSource = LottieVisualSource.CreateFromString(stringAnimation);
                    break;
                case AnimationSource.Json:
                    //if (animation is string jsonAnimation)
                    //    animatedVisualSource = LottieVisualSource.CreateFromString(jsonAnimation);
                    break;
                case AnimationSource.Stream:
                    animatedVisualSource.TrySetAnimation(animation);
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
                        animatedVisualSource.TrySetAnimation(stream);
                    }
                    break;
                default:
                    break;
            }
        }
        
        public static void TrySetAnimation(this IAnimatedVisualSource animatedVisualSource, object animation)
        {
            switch (animation)
            {
                //case int intAnimation:
                    //animatedVisualSource = new LottieVisualSource { UriSource = new Uri(intAnimation) };
                //    break;
                case string stringAnimation:
                    animatedVisualSource = LottieVisualSource.CreateFromString(stringAnimation);
                    break;
                case Stream streamAnimation:
                    //TODO: api for this will be added in next Lottie UWP update
                    //var source = new LottieVisualSource();
                    //source.SetSourceAsync(streamAnimation);
                    //animatedVisualSource = source;
                    break;
                case null:
                    animatedVisualSource = null;
                    break;
                default:
                    break;
            }
        }
    }
}
