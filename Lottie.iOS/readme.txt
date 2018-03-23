---------------------------------
Lottie
---------------------------------

Lottie is a mobile library for Android and iOS that parses Adobe After Effects animations exported as json with Bodymovin and renders them natively on mobile!

Using Lottie on Xamarin.iOS:

LOTAnimationView animation = LOTAnimationView.AnimationNamed("LottieLogo1");
this.View.AddSubview(animation);
animation.PlayWithCompletion((animationFinished) => {
  // Do Something
});

//You can also use the awaitable version
var animationFinished = await animation.PlayAsync();

Star on Github if this project helps you: https://github.com/martijn00/LottieXamarin