using System.IO;
using LottieSharp;

namespace Lottie.Forms.Platforms.Wpf
{
    public static class AnimationViewExtensions
    {
        public static LottieComposition GetAnimation(this AnimationView animationView)
        {
            LottieComposition composition = null;
            switch (animationView.AnimationSource)
            {
                case AnimationSource.AssetOrBundle:
                    if (animationView.Animation is string assetAnnimation)
                    {
                        var assets = "Assets";

                        if (!string.IsNullOrEmpty(animationView.ImageAssetsFolder))
                        {
                            assets = animationView.ImageAssetsFolder;
                        }

                        var path = $"ms-appx:///{assets}/{assetAnnimation}";
                        composition = animationView.GetAnimation(path);
                    }
                    break;
                case AnimationSource.Url:
                    if (animationView.Animation is string stringAnimation)
                        composition = animationView.GetAnimation(stringAnimation);
                    break;
                case AnimationSource.Json:
                    //if (animation is string jsonAnimation)
                    //    animatedVisualSource = LottieVisualSource.CreateFromString(jsonAnimation);
                    break;
                case AnimationSource.Stream:
                    composition = animationView.GetAnimation(animationView.Animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    if (animationView.Animation is string embeddedAnimation)
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

        public static LottieComposition GetAnimation(this AnimationView animationView, object animation)
        {
            LottieComposition composition = null;
            switch (animation)
            {
                //case int intAnimation:
                //animatedVisualSource = new LottieVisualSource { UriSource = new Uri(intAnimation) };
                //    break;
                case string stringAnimation:
                    composition = LottieComposition.Factory.FromFileSync(stringAnimation);
                    break;
                case Stream streamAnimation:
                    //TODO: api for this will be added in next Lottie UWP update
                    //var source = new LottieVisualSource();
                    //source.SetSourceAsync(streamAnimation);
                    //animatedVisualSource = source;
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
