# LottieXamarin
Lottie is a mobile library for Android and iOS (Xamarin and .NET 6) that parses [Adobe After Effects](http://www.adobe.com/products/aftereffects.html) animations exported as json with [Bodymovin](https://github.com/bodymovin/bodymovin) and renders them natively on mobile!

# Support

* Feel free to open an issue. Make sure to use one of the templates!
* Commercial support is available. Integration with your app or services, samples, feature request, etc. Email: [hello@baseflow.com](mailto:hello@baseflow.com)
* Powered by: [baseflow.com](https://baseflow.com)

## Download

Android: [![NuGet Badge](https://buildstats.info/nuget/Com.Airbnb.Android.Lottie)](https://www.nuget.org/packages/Com.Airbnb.Android.Lottie/)
iOS: [![NuGet Badge](https://buildstats.info/nuget/Com.Airbnb.iOS.Lottie)](https://www.nuget.org/packages/Com.Airbnb.iOS.Lottie/)
Xamarin.Forms: [![NuGet Badge](https://buildstats.info/nuget/Com.Airbnb.Xamarin.Forms.Lottie)](https://www.nuget.org/packages/Com.Airbnb.Xamarin.Forms.Lottie/)


For the first time, designers can create **and ship** beautiful animations without an engineer painstakingly recreating it by hand. They say a picture is worth 1,000 words so here are 13,000:

![Example1](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example1.gif)


![Example2](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example2.gif)


![Example3](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example3.gif)


![Community](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Community%202_3.gif)


![Example4](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example4.gif)

All of these animations were created in After Effects, exported with Bodymovin, and rendered natively with no additional engineering effort.

[Bodymovin](https://github.com/bodymovin/bodymovin) is an After Effects plugin created by Hernan Torrisi that exports After effects files as json and includes a javascript web player. We've built on top of his great work to extend its usage to Android, iOS, and React Native.

Read more about it on our [blog post](http://airbnb.design/introducing-lottie/)
Or get in touch on Twitter ([gpeal8](https://twitter.com/gpeal8)) or via lottie@airbnb.com

## Sample App

You can build the sample app yourself or download it from the [Play Store](https://play.google.com/store/apps/details?id=com.airbnb.lottie). The sample app includes some built in animations but also allows you to load an animation from internal storage or from a url.

## Using Lottie for Xamarin.Forms

A normal sample is:

```c#
<forms:AnimationView
    x:Name="animationView"
    Animation="LottieLogo1.json"
    AnimationSource="AssetOrBundle"
    Command="{Binding ClickCommand}"
    VerticalOptions="FillAndExpand"
    HorizontalOptions="FillAndExpand" />
```

All possible options are:

```c#
<forms:AnimationView 
    x:Name="animationView"
    Animation="LottieLogo1.json"
    AnimationSource="AssetOrBundle"
    AutoPlay="True"
    CacheComposition="True"
    Clicked="animationView_Clicked"
    Command="{Binding ClickCommand}"
    FallbackResource="{Binding Image}"
    ImageAssetsFolder="Assets/lottie"
    IsAnimating="{Binding IsAnimating}"
    MaxFrame="100"
    MaxProgress="100"
    MinFrame="0"
    MinProgress="0"
    OnAnimationLoaded="animationView_OnAnimationLoaded"
    OnAnimationUpdate="animationView_OnAnimationUpdate"
    OnFailure="animationView_OnFailure"
    OnFinishedAnimation="animationView_OnFinishedAnimation"
    OnPauseAnimation="animationView_OnPauseAnimation"
    OnPlayAnimation="animationView_OnPlayAnimation"
    OnRepeatAnimation="animationView_OnRepeatAnimation"
    OnResumeAnimation="animationView_OnResumeAnimation"
    OnStopAnimation="animationView_OnStopAnimation"
    Progress="{Binding Progress}"
    RepeatCount="3"
    RepeatMode="Restart"
    Scale="1"
    Speed="1"
    VerticalOptions="FillAndExpand"
    HorizontalOptions="FillAndExpand" />
```

## Using Lottie for Xamarin Android
Lottie supports Ice Cream Sandwich (API 14) and above.
The simplest way to use it is with LottieAnimationView:

```xml
<com.airbnb.lottie.LottieAnimationView
        android:id="@+id/animation_view"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        app:lottie_fileName="hello-world.json"
        app:lottie_loop="true"
        app:lottie_autoPlay="true" />
```

Or you can load it programatically in multiple ways.
From a json asset in app/src/main/assets:
```c#
LottieAnimationView animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
animationView.SetAnimation("hello-world.json");
animationView.Loop = true;
```
This method will load the file and parse the animation in the background and asynchronously start rendering once completed.

If you want to reuse an animation such as in each item of a list or load it from a network request JSONObject:
```c#
 LottieAnimationView animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
 ...
 LottieComposition composition = LottieComposition.Factory.FromJson(Resources, jsonObject, (composition) => 
 {
     animationView.SetComposition(composition);
     animationView.PlayAnimation();
 });
```

You can then control the animation or add listeners:
```c#
animationView.AddAnimatorUpdateListener(animationListener);
animationView.PlayAnimation();
...
if (animationView.IsAnimating) 
{
    // Do something.
}
...
animationView.Progress = 0.5f;
...
// Custom animation speed or duration.
ValueAnimator animator = ValueAnimator.OfFloat(0f, 1f).SetDuration(500);
animator.Update += (sender, e) => animationView.Progress = (float)e.Animation.AnimatedValue;
animator.Start();
...
animationView.CancelAnimation();
```


Under the hood, `LottieAnimationView` uses `LottieDrawable` to render its animations. If you need to, you can use the the drawable form directly:
```c#
LottieDrawable drawable = new LottieDrawable();
LottieComposition.Factory.FromAssetFileName(Context, "hello-world.json", (composition) => {
    drawable.SetComposition(composition);
});
```

If your animation will be frequently reused, `LottieAnimationView` has an optional caching strategy built in. Use `LottieAnimationView#SetAnimation(String, CacheStrategy)`. `CacheStrategy` can be `Strong`, `Weak`, or `None` to have `LottieAnimationView` hold a strong or weak reference to the loaded and parsed animation. 

You can also use the awaitable version of LottieComposition's asynchronous methods:
```c#
var composition = await LottieComposition.Factory.FromAssetFileNameAsync(this.Context, assetName);
..
var composition = await LottieComposition.Factory.FromJsonAsync(Resources, jsonObject);
...
var composition = await LottieComposition.Factory.FromInputStreamAsync(this.Context, stream);
```

### Image Support
You can animate images if your animation is loaded from assets and your image file is in a 
subdirectory of assets. Just call `SetImageAssetsFolder` on `LottieAnimationView` or 
`LottieDrawable` with the relative folder inside of assets and make sure that the images that 
bodymovin export are in that folder with their names unchanged (should be img_#).
If you use `LottieDrawable` directly, you must call `RecycleBitmaps` when you are done with it.

If you need to provide your own bitmaps if you downloaded them from the network or something, you
 can provide a delegate to do that:
 ```c#
animationView.SetImageAssetDelegate((LottieImageAsset asset) =>
{
    retun GetBitmap(asset);
});
```


## Using Lottie for Xamarin iOS
Lottie supports iOS 8 and above.
Lottie animations can be loaded from bundled JSON or from a URL

The simplest way to use it is with LOTAnimationView:
```c#
LOTAnimationView animation = LOTAnimationView.AnimationNamed("LottieLogo1");
this.View.AddSubview(animation);
animation.PlayWithCompletion((animationFinished) => {
  // Do Something
});
//You can also use the awaitable version
//var animationFinished = await animation.PlayAsync();
```

Or you can load it programmatically from a NSUrl
```c#
LOTAnimationView animation = new LOTAnimationView(new NSUrl(url));
this.View.AddSubview(animation);
```

Lottie supports the iOS `UIViewContentModes` ScaleAspectFit and ScaleAspectFill

You can also set the animation progress interactively.
```c#
CGPoint translation = gesture.GetTranslationInView(this.View);
nfloat progress = translation.Y / this.View.Bounds.Size.Height;
animationView.AnimationProgress = progress;
```

Want to mask arbitrary views to animation layers in a Lottie View?
Easy-peasy as long as you know the name of the layer from After Effects

```c#
UIView snapshot = this.View.SnapshotView(afterScreenUpdates: true);
lottieAnimation.AddSubview(snapshot, layer: "AfterEffectsLayerName");
```

Lottie comes with a `UIViewController` animation-controller for making custom viewController transitions!

```c#
#region View Controller Transitioning
public class LOTAnimationTransitionDelegate : UIViewControllerTransitioningDelegate
{
    public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
    {
        LOTAnimationTransitionController animationController =
            new LOTAnimationTransitionController(
            animation: "vcTransition1",
            fromLayer: "outLayer",
            toLayer: "inLayer");

        return animationController;
    }

    public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
    {
        LOTAnimationTransitionController animationController = 
            new LOTAnimationTransitionController(
                animation: "vcTransition2",
                fromLayer: "outLayer",
                toLayer: "inLayer");

        return animationController;
    } 
}
#endregion
```

If your animation will be frequently reused, `LOTAnimationView` has an built in LRU Caching Strategy.


## Supported After Effects Features

### Keyframe Interpolation

---

* Linear Interpolation

* Bezier Interpolation

* Hold Interpolation

* Rove Across Time

* Spatial Bezier

### Solids

---

* Transform Anchor Point

* Transform Position

* Transform Scale

* Transform Rotation

* Transform Opacity

### Masks

---

* Path

* Opacity

* Multiple Masks (additive)

### Track Mattes

---

* Alpha Matte

### Parenting

---

* Multiple Parenting

* Nulls

### Shape Layers

---

* Rectangle (All properties)

* Ellipse (All properties)

* Polystar (All properties)

* Polygon (All properties. Integer point values only.)

* Path (All properties)

* Anchor Point

* Position

* Scale

* Rotation

* Opacity

* Group Transforms (Anchor point, position, scale etc)

* Multiple paths in one group

#### Stroke (shape layer)

---

* Stroke Color

* Stroke Opacity

* Stroke Width

* Line Cap

* Dashes

#### Fill (shape layer)

---

* Fill Color

* Fill Opacity

#### Trim Paths (shape layer)

---

* Trim Paths Start

* Trim Paths End

* Trim Paths Offset

## Performance and Memory
1. If the composition has no masks or mattes then the performance and memory overhead should be quite good. No bitmaps are created and most operations are simple canvas draw operations.
2. If the composition has mattes, 2-3 bitmaps will be created at the composition size. The bitmaps are created automatically by lottie when the animation view is added to the window and recycled when it is removed from the window. For this reason, it is not recommended to use animations with masks or mattes in a RecyclerView because it will cause significant bitmap churn. In addition to memory churn, additional bitmap.eraseColor() and canvas.drawBitmap() calls are necessary for masks and mattes which will slow down the performance of the animation. For small animations, the performance hit should not be large enough to be obvious when actually used.
4. If you are using your animation in a list, it is recommended to use a CacheStrategy in LottieAnimationView.setAnimation(String, CacheStrategy) so the animations do not have to be deserialized every time.

## Try it out
Clone this repository and run the LottieSample module to see a bunch of sample animations. The JSON files for them are located in [LottieSample/src/main/assets](https://github.com/airbnb/lottie-android/tree/master/LottieSample/src/main/assets) and the orignal After Effects files are located in [/After Effects Samples](https://github.com/airbnb/lottie-android/tree/master/After%20Effects%20Samples)

The sample app can also load json files at a given url or locally on your device (like Downloads or on your sdcard).

## Community Contributions
 * [Xamarin bindings](https://github.com/martijn00/LottieXamarin)
 * [NativeScript bindings](https://github.com/bradmartin/nativescript-lottie)
 * [Appcelerator Titanium bindings](https://github.com/m1ga/ti.animation)
 
## Community Contributors
 * [modplug](https://github.com/modplug)
 * [fabionuno](https://github.com/fabionuno)
 * [matthewrdev](https://github.com/matthewrdev)
 * [alexsorokoletov](https://github.com/alexsorokoletov)
 * [jzeferino](https://github.com/jzeferino)
 
## Alternatives
1. Build animations by hand. Building animations by hand is a huge time commitment for design and engineering across Android and iOS. It's often hard or even impossible to justify spending so much time to get an animation right.
2. [Facebook Keyframes](https://github.com/facebookincubator/Keyframes). Keyframes is a wonderful new library from Facebook that they built for reactions. However, Keyframes doesn't support some of Lottie's features such as masks, mattes, trim paths, dash patterns, and more.
2. Gifs. Gifs are more than double the size of a bodymovin JSON and are rendered at a fixed size that can't be scaled up to match large and high density screens.
3. Png sequences. Png sequences are even worse than gifs in that their file sizes are often 30-50x the size of the bodymovin json and also can't be scaled up.

## Why is it called Lottie?
Lottie is named after a German film director and the foremost pioneer of silhouette animation. Her best known films are The Adventures of Prince Achmed (1926) – the oldest surviving feature-length animated film, preceding Walt Disney's feature-length Snow White and the Seven Dwarfs (1937) by over ten years
[The art of Lotte Reineger](https://www.youtube.com/watch?v=LvU55CUw5Ck&feature=youtu.be)

## Contributing
Contributors are more than welcome. Just upload a PR with a description of your changes.
Lottie uses [Facebook screenshot tests for Android](https://github.com/facebook/screenshot-tests-for-android) to identify pixel level changes/breakages. Please run `./gradlew --daemon recordMode screenshotTests` before uploading a PR to ensure that nothing has broken. Use a Nexus 5 emulator running Lollipop for this. Changed screenshots will show up in your git diff if you have.

If you would like to add more JSON files and screenshot tests, feel free to do so and add the test to `LottieTest`.

## Issues or feature requests?
File github issues for anything that is unexpectedly broken. If an After Effects file is not working, please attach it to your issue. Debugging without the original file is much more difficult.

# To build the source code from command line

* msbuild Lottie.sln /t:restore
* msbuild Lottie.sln /p:Configuration=Release 
