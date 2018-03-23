---------------------------------
Lottie
---------------------------------

Lottie is a mobile library for Android and iOS that parses Adobe After Effects animations exported as json with Bodymovin and renders them natively on mobile!

Using Lottie on Xamarin.Forms:

<forms:AnimationView 
	x:Name="animationView" 
	Grid.Row="1"
	Animation="LottieLogo1.json" 
	Loop="false" 
	AutoPlay="false"
	OnFinish="Handle_OnFinish"
	PlaybackStartedCommand="{Binding PlayingCommand}"
	PlaybackFinishedCommand="{Binding FinishedCommand}" 
	ClickedCommand="{Binding ClickedCommand}"
	VerticalOptions="FillAndExpand" 
	HorizontalOptions="FillAndExpand" />

Star on Github if this project helps you: https://github.com/martijn00/LottieXamarin