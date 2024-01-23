using System.IO;
using Microsoft.Toolkit.Uwp.UI.Lottie;
using Microsoft.UI.Xaml.Controls;

namespace Lottie.Forms.Platforms.Uap
{
    public static class AnimationViewExtensions
    {
        public static async Task<IAnimatedVisualSource> GetAnimationAsync(this AnimationView animationView)
        {
            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

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
                        animatedVisualSource = await animationView.GetAnimationAsync(path);
                    }
                    break;
                case AnimationSource.Url:
                    if (animationView.Animation is string stringAnimation)
                        animatedVisualSource = await animationView.GetAnimationAsync(stringAnimation);
                    break;
                case AnimationSource.Json:
                    //if (animation is string jsonAnimation)
                    //    animatedVisualSource = LottieVisualSource.CreateFromString(jsonAnimation);
                    break;
                case AnimationSource.Stream:
                    animatedVisualSource = await animationView.GetAnimationAsync(animationView.Animation);
                    break;
                case AnimationSource.EmbeddedResource:
                    animatedVisualSource = await animationView.GetAnimationAsync(animationView.GetStreamFromAssembly());
                    break;
                default:
                    break;
            }
            return animatedVisualSource;
        }

        public static async Task<IAnimatedVisualSource> GetAnimationAsync(this AnimationView animationView, object animation)
        {
            if (animationView == null)
                throw new ArgumentNullException(nameof(animationView));

            IAnimatedVisualSource animatedVisualSource = null;
            switch (animation)
            {
                case string stringAnimation:
                    animatedVisualSource = LottieVisualSource.CreateFromString(stringAnimation);
                    break;
                case Stream streamAnimation:
                    var source = new LottieVisualSource();
                    await source.SetSourceAsync(streamAnimation.AsInputStream());
                    animatedVisualSource = source;
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
