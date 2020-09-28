---------------------------------
Lottie
---------------------------------

Lottie is a mobile library for Android and iOS that parses Adobe After Effects animations exported as json with Bodymovin and renders them natively on mobile!

Using Lottie on Xamarin.Forms:

Namespace:

xmlns:forms="clr-namespace:Lottie.Forms;assembly=Lottie.Forms" 

Example:

<forms:AnimationView
    x:Name="animationView"
    Animation="LottieLogo1.json"
    AnimationSource="AssetOrBundle"
    Command="{Binding ClickCommand}"
    RepeatCount="3"
    RepeatMode="Restart"
    VerticalOptions="FillAndExpand"
    HorizontalOptions="FillAndExpand" />

All options:

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


---------------------------------
Star on Github if this project helps you: https://github.com/Baseflow/LottieXamarin

Commercial support is available. Integration with your app or services, samples, feature request, etc. Email: hello@baseflow.com
Powered by: https://baseflow.com
---------------------------------