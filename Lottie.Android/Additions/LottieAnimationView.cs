using System;
using Android.Graphics;

namespace Com.Airbnb.Lottie
{
    partial class LottieAnimationView
    {
        /// <summary>
        /// Delegate to handle the loading of bitmaps that are not packaged in the assets of your app.
        /// </summary>
        public void SetImageAssetDelegate(Func<LottieImageAsset, Bitmap> funcAssetLoad)
        {
            this.SetImageAssetDelegate(new ImageAssetDelegateImpl(funcAssetLoad));
        }

        internal sealed class ImageAssetDelegateImpl : Java.Lang.Object, IImageAssetDelegate
        {
            private readonly Func<LottieImageAsset, Bitmap> funcAssetLoad;

            public ImageAssetDelegateImpl(Func<LottieImageAsset, Bitmap> funcAssetLoad)
            {
                this.funcAssetLoad = funcAssetLoad;
            }

            public Bitmap FetchBitmap(LottieImageAsset asset)
            {
                return this.funcAssetLoad(asset);
            }
        }
    }
}
