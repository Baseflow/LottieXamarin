using System;
using Android.Content;
using Org.Json;

namespace Com.Airbnb.Lottie.Model
{
    partial class LottieComposition
    {
        /// <summary>
        /// Loads a composition from a file stored in /assets.
        /// </summary>
        public static LottieComposition.ICancellable FromAssetFileName(Context context, String fileName, Action<LottieComposition> onLoaded)
        {
            return LottieComposition.FromAssetFileName(context, fileName, new ActionCompositionLoaded(onLoaded));
        }


        /// <summary>
        /// Loads a composition from an arbitrary input stream.
        /// </summary>
        public static LottieComposition.ICancellable FromInputStream(Context context, System.IO.Stream stream, Action<LottieComposition> onLoaded)
        {
            return LottieComposition.FromInputStream(context, stream, new ActionCompositionLoaded(onLoaded));
        }


        /// <summary>
        ///  Loads a composition from a raw json object. This is useful for animations loaded from the network.
        /// </summary>
        public static LottieComposition.ICancellable FromJson(Android.Content.Res.Resources res, JSONObject json, Action<LottieComposition> onLoaded)
        {
            return LottieComposition.FromJson(res, json, new ActionCompositionLoaded(onLoaded));
        }



        private class ActionCompositionLoaded : Java.Lang.Object, LottieComposition.IOnCompositionLoadedListener
        {
            private readonly Action<LottieComposition> onLoaded;

            public ActionCompositionLoaded(Action<LottieComposition> onLoaded)
            {
                this.onLoaded = onLoaded;
            }

            public void OnCompositionLoaded(LottieComposition compostion)
            {
                if (onLoaded != null)
                {
                    onLoaded(compostion);
                }
            }
        }
    }
}
