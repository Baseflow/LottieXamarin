using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Org.Json;

namespace Com.Airbnb.Lottie
{
    partial class LottieComposition
    {
        partial class Factory
        {
            /// <summary>
            /// Asynchronously loads a composition from a file stored in /assets.
            /// </summary>
            public static ICancellable FromAssetFileName(Context context, String fileName, Action<LottieComposition> onLoaded)
            {
                return Factory.FromAssetFileName(context, fileName, new ActionCompositionLoaded(onLoaded));
            }

            /// <summary>
            /// Asynchronously loads a composition from an arbitrary input stream.
            /// </summary>
            public static ICancellable FromInputStream(Context context, System.IO.Stream stream, Action<LottieComposition> onLoaded)
            {
                return Factory.FromInputStream(context, stream, new ActionCompositionLoaded(onLoaded));
            }

            /// <summary>
            ///  Asynchronously loads a composition from a raw json object. This is useful for animations loaded from the network.
            /// </summary>
            public static ICancellable FromJson(Android.Content.Res.Resources res, JSONObject json, Action<LottieComposition> onLoaded)
            {
                return Factory.FromJson(res, json, new ActionCompositionLoaded(onLoaded));
            }

            ///// <summary>
            ///// Asynchronously loads a composition from a file stored in /assets.
            ///// </summary>
            public static Task<LottieComposition> FromAssetFileNameAsync(Context context, String fileName)
            {
                return FromAssetFileNameAsync(context, fileName, CancellationToken.None);
            }

            /////// <summary>
            /////// Asynchronously loads a composition from a file stored in /assets.
            /////// </summary>
            public static Task<LottieComposition> FromAssetFileNameAsync(Context context, String fileName, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<LottieComposition>(cancellationToken);

                var tcs = new TaskCompletionSource<LottieComposition>();
                var cancelable = Factory.FromAssetFileName(context, fileName, (composition) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    tcs.SetResult(composition);
                });

                cancellationToken.Register(() =>
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        cancelable.Cancel();
                        tcs.TrySetCanceled(cancellationToken);
                    }
                });

                return tcs.Task;
            }

            ///// <summary>
            ///// Asynchronously loads a composition from an arbitrary input stream.
            ///// </summary>
            public static Task<LottieComposition> FromInputStreamAsync(Context context, System.IO.Stream stream)
            {
                return FromInputStreamAsync(context, stream, CancellationToken.None);
            }

            ///// <summary>
            ///// Asynchronously loads a composition from an arbitrary input stream.
            ///// </summary>
            public static Task<LottieComposition> FromInputStreamAsync(Context context, System.IO.Stream stream, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<LottieComposition>(cancellationToken);

                var tcs = new TaskCompletionSource<LottieComposition>();
                var cancelable = Factory.FromInputStream(context, stream, (composition) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    tcs.SetResult(composition);
                });

                cancellationToken.Register(() =>
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        cancelable.Cancel();
                        tcs.TrySetCanceled(cancellationToken);
                    }
                });

                return tcs.Task;
            }

            ///// <summary>
            /////  Asynchronously loads a composition from a raw json object. This is useful for animations loaded from the network.
            ///// </summary>
            public static Task<LottieComposition> FromJsonAsync(Resources res, JSONObject json)
            {
                return FromJsonAsync(res, json, CancellationToken.None);
            }

            ///// <summary>
            /////  Asynchronously loads a composition from a raw json object. This is useful for animations loaded from the network.
            ///// </summary>
            public static Task<LottieComposition> FromJsonAsync(Resources res, JSONObject json, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled<LottieComposition>(cancellationToken);

                var tcs = new TaskCompletionSource<LottieComposition>();
                var cancelable = Factory.FromJson(res, json, (composition) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    tcs.SetResult(composition);
                });

                cancellationToken.Register(() =>
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        cancelable.Cancel();
                        tcs.TrySetCanceled(cancellationToken);
                    }
                });

                return tcs.Task;
            }

            internal sealed class ActionCompositionLoaded : Java.Lang.Object, IOnCompositionLoadedListener
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
}