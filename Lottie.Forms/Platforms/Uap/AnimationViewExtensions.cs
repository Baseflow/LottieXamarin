using System.IO;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Microsoft.UI.Xaml.Controls;

namespace Lottie.Forms.Platforms.Uap
{
    public static class AnimationViewExtensions
    {
        public static IAnimatedVisualSource GetAnimation(this AnimationView animationView)
        {
            IAnimatedVisualSource animatedVisualSource = null;
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
                        animatedVisualSource = animationView.GetAnimation(path);
                    }
                    break;
                case AnimationSource.Url:
                    if (animationView.Animation is string stringAnimation)
                        animatedVisualSource = animationView.GetAnimation(stringAnimation);
                    break;
                case AnimationSource.Json:
                    //if (animation is string jsonAnimation)
                    //    animatedVisualSource = LottieVisualSource.CreateFromString(jsonAnimation);
                    break;
                case AnimationSource.Stream:
                    animatedVisualSource = animationView.GetAnimation(animationView.Animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    animatedVisualSource = animationView.GetAnimation(animationView.GetStreamFromAssembly());
                    break;
                default:
                    break;
            }
            return animatedVisualSource;
        }

        public static IAnimatedVisualSource GetAnimation(this AnimationView animationView, object animation)
        {
            IAnimatedVisualSource animatedVisualSource = null;
            switch (animation)
            {
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
            return animatedVisualSource;
        }
    }
}
