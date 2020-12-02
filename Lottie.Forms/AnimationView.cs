using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lottie.Forms
{
    public class AnimationView : View
    {
        //public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image),
        //    typeof(ImageSource), typeof(AnimationView), default(ImageSource));

        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation),
            typeof(object), typeof(AnimationView), default(object));

        public static readonly BindableProperty AnimationSourceProperty = BindableProperty.Create(nameof(Lottie.Forms.AnimationSource),
            typeof(AnimationSource), typeof(AnimationView), Forms.AnimationSource.AssetOrBundle);

        public static readonly BindableProperty CacheCompositionProperty = BindableProperty.Create(nameof(CacheComposition),
            typeof(bool), typeof(AnimationView), true);

        public static readonly BindableProperty FallbackResourceProperty = BindableProperty.Create(nameof(FallbackResource),
            typeof(ImageSource), typeof(AnimationView), default(ImageSource));

        //public static readonly BindableProperty CompositionProperty = BindableProperty.Create(nameof(Composition),
        //    typeof(ILottieComposition), typeof(AnimationView), default(ILottieComposition));

        public static readonly BindableProperty MinFrameProperty = BindableProperty.Create(nameof(MinFrame),
            typeof(int), typeof(AnimationView), int.MinValue);

        public static readonly BindableProperty MinProgressProperty = BindableProperty.Create(nameof(MinProgress),
            typeof(float), typeof(AnimationView), float.MinValue);

        public static readonly BindableProperty MaxFrameProperty = BindableProperty.Create(nameof(MaxFrame),
            typeof(int), typeof(AnimationView), int.MinValue);

        public static readonly BindableProperty MaxProgressProperty = BindableProperty.Create(nameof(MaxProgress),
            typeof(float), typeof(AnimationView), float.MinValue);

        public static readonly BindableProperty SpeedProperty = BindableProperty.Create(nameof(Speed),
            typeof(float), typeof(AnimationView), 1.0f);

        public static readonly BindableProperty RepeatModeProperty = BindableProperty.Create(nameof(RepeatMode),
            typeof(RepeatMode), typeof(AnimationView), Lottie.Forms.RepeatMode.Restart);

        public static readonly BindableProperty RepeatCountProperty = BindableProperty.Create(nameof(RepeatCount),
            typeof(int), typeof(AnimationView), 0);

        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating),
            typeof(bool), typeof(AnimationView), false);

        public static readonly BindableProperty ImageAssetsFolderProperty = BindableProperty.Create(nameof(ImageAssetsFolder),
            typeof(string), typeof(AnimationView), default(string));

        //public static new readonly BindableProperty ScaleProperty = BindableProperty.Create(nameof(Scale),
        //    typeof(float), typeof(AnimationView), 1.0f);

        public static readonly BindableProperty FrameProperty = BindableProperty.Create(nameof(Frame),
            typeof(int), typeof(AnimationView), default(int));

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress),
            typeof(float), typeof(AnimationView), 0.0f);

        //TODO: Maybe make TimeSpan
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration),
            typeof(long), typeof(AnimationView), default(long));

        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay),
            typeof(bool), typeof(AnimationView), true);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(AnimationView));

        /// <summary>
        /// Returns the duration of an animation (Frames / FrameRate * 1000)
        /// </summary>
        public long Duration
        {
            get { return (long)GetValue(DurationProperty); }
            internal set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Indicates if a Lottie Animation should be cached
        /// </summary>
        public bool CacheComposition
        {
            get { return (bool)GetValue(CacheCompositionProperty); }
            set { SetValue(CacheCompositionProperty, value); }
        }

        /// <summary>
        /// Set the Animation that you want to play. This can be a URL (either local path or remote), Json string, or Stream
        /// </summary>
        public object Animation
        {
            get { return (object)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Indicates where the Animation is located and from which source it should be loaded
        /// Default value is AssetOrBundle
        /// </summary>
        public AnimationSource AnimationSource
        {
            get { return (AnimationSource)GetValue(AnimationSourceProperty); }
            set { SetValue(AnimationSourceProperty, value); }
        }

        /// <summary>
        /// Used in case an animations fails to load
        /// </summary>
        public ImageSource FallbackResource
        {
            get { return (ImageSource)GetValue(FallbackResourceProperty); }
            set { SetValue(FallbackResourceProperty, value); }
        }

        //public ILottieComposition Composition
        //{
        //    get { return (ILottieComposition)GetValue(CompositionProperty); }
        //    set { SetValue(CompositionProperty, value); }
        //}

        /// <summary>
        /// Sets or gets the minimum frame that the animation will start from when playing or looping.
        /// </summary>
        public int MinFrame
        {
            get { return (int)GetValue(MinFrameProperty); }
            set { SetValue(MinFrameProperty, value); }
        }

        /// <summary>
        /// Sets or gets the minimum progress that the animation will start from when playing or looping.
        /// </summary>
        public float MinProgress
        {
            get { return (float)GetValue(MinProgressProperty); }
            set { SetValue(MinProgressProperty, value); }
        }

        /// <summary>
        /// Sets or gets the maximum frame that the animation will end at when playing or looping.
        /// </summary>
        public int MaxFrame
        {
            get { return (int)GetValue(MaxFrameProperty); }
            set { SetValue(MaxFrameProperty, value); }
        }

        /// <summary>
        /// Sets or gets the maximum progress that the animation will end at when playing or looping.
        /// </summary>
        public float MaxProgress
        {
            get { return (float)GetValue(MaxProgressProperty); }
            set { SetValue(MaxProgressProperty, value); }
        }

        /// <summary>
        /// Returns the current playback speed. This will be < 0 if the animation is playing backwards.
        /// </summary>
        public float Speed
        {
            get { return (float)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        /// <summary>
        /// Defines what this animation should do when it reaches the end. 
        /// This setting is applied only when the repeat count is either greater than 0 or INFINITE.
        /// Defaults to RESTART.
        /// </summary>
        public RepeatMode RepeatMode
        {
            get { return (RepeatMode)GetValue(RepeatModeProperty); }
            set { SetValue(RepeatModeProperty, value); }
        }

        /// <summary>
        /// Sets how many times the animation should be repeated. If the repeat count is 0, the animation is never repeated.
        /// If the repeat count is greater than 0 or INFINITE, the repeat mode will be taken into account.
        /// The repeat count is 0 by default.
        /// </summary>
        public int RepeatCount
        {
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        /// <summary>
        /// Indicates if the Animation is playing
        /// </summary>
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            internal set { SetValue(IsAnimatingProperty, value); }
        }

        /// <summary>
        /// If you use image assets, you must explicitly specify the folder in assets/ in which they are located because bodymovin uses the name filenames across all compositions
        /// </summary>
        public string ImageAssetsFolder
        {
            get { return (string)GetValue(ImageAssetsFolderProperty); }
            set { SetValue(ImageAssetsFolderProperty, value); }
        }

        /// <summary>
        /// Set the scale on the current composition. 
        /// The only cost of this function is re-rendering the current frame so you may call it frequent to scale something up or down.
        /// </summary>
        //public new float Scale
        //{
        //    get { return (float)GetValue(ScaleProperty); }
        //    set { SetValue(ScaleProperty, value); }
        //}

        /// <summary>
        /// Sets the progress to the specified frame.
        /// If the composition isn't set yet, the progress will be set to the frame when it is.
        /// </summary>
        public int Frame
        {
            get { return (int)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        /// <summary>
        /// Returns the current progress of the animation
        /// </summary>
        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        /// <summary>
        /// When true the Lottie animation will automatically start playing when loaded
        /// </summary>
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        /// <summary>
        /// Will be called when the view is clicked
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Called when the Lottie animation starts playing
        /// </summary>
        public event EventHandler OnPlayAnimation;

        /// <summary>
        /// Called when the Lottie animation is paused
        /// </summary>
        public event EventHandler OnPauseAnimation;

        /// <summary>
        /// Called when the Lottie animation is resumed after pausing
        /// </summary>
        public event EventHandler OnResumeAnimation;

        /// <summary>
        /// Called when the Lottie animation is stopped
        /// </summary>
        public event EventHandler OnStopAnimation;

        /// <summary>
        /// Called when the Lottie animation is repeated
        /// </summary>
        public event EventHandler OnRepeatAnimation;

        /// <summary>
        /// Called when the Lottie animation is clicked
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Called when the Lottie animation is playing with the current progress
        /// </summary>
        public event EventHandler<float> OnAnimationUpdate;

        /// <summary>
        /// Called when the Lottie animation is loaded with the Lottie Composition as parameter
        /// </summary>
        public event EventHandler<object> OnAnimationLoaded;

        /// <summary>
        /// Called when the animation fails to load or when an exception happened when trying to play
        /// </summary>
        public event EventHandler<Exception> OnFailure;

        /// <summary>
        /// Called when the Lottie animation is finished playing
        /// </summary>
        public event EventHandler OnFinishedAnimation;

        internal void InvokePlayAnimation()
        {
            OnPlayAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeResumeAnimation()
        {
            OnResumeAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeStopAnimation()
        {
            OnStopAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokePauseAnimation()
        {
            OnPauseAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeRepeatAnimation()
        {
            OnRepeatAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeAnimationUpdate(float progress)
        {
            OnAnimationUpdate?.Invoke(this, progress);
        }

        internal void InvokeAnimationLoaded(object animation)
        {
            OnAnimationLoaded?.Invoke(this, animation);
        }

        internal void InvokeFailure(Exception ex)
        {
            OnFailure?.Invoke(this, ex);
        }

        internal void InvokeFinishedAnimation()
        {
            OnFinishedAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeClick()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
            Command.ExecuteCommandIfPossible(this);
        }

        internal ICommand PlayCommand { get; set; }
        internal ICommand PauseCommand { get; set; }
        internal ICommand ResumeCommand { get; set; }
        internal ICommand StopCommand { get; set; }
        internal ICommand ClickCommand { get; set; }
        internal ICommand PlayMinAndMaxFrameCommand { get; set; }
        internal ICommand PlayMinAndMaxProgressCommand { get; set; }
        internal ICommand ReverseAnimationSpeedCommand { get; set; }

        /// <summary>
        /// Simulate a click action on the view
        /// </summary>
        public void Click()
        {
            ClickCommand.ExecuteCommandIfPossible(this);
        }

        /// <summary>
        /// Plays the animation from the beginning. If speed is < 0, it will start at the end and play towards the beginning
        /// </summary>
        public void PlayAnimation()
        {
            PlayCommand.ExecuteCommandIfPossible();
        }

        /// <summary>
        /// Continues playing the animation from its current position. If speed < 0, it will play backwards from the current position.
        /// </summary>
        public void ResumeAnimation()
        {
            ResumeCommand.ExecuteCommandIfPossible();
        }

        /// <summary>
        /// Will stop and reset the currently playing animation
        /// </summary>
        public void StopAnimation()
        {
            StopCommand.ExecuteCommandIfPossible();
        }

        /// <summary>
        /// Will pause the currently playing animation. Call ResumeAnimation to continue
        /// </summary>
        public void PauseAnimation()
        {
            PauseCommand.ExecuteCommandIfPossible();
        }

        public void PlayMinAndMaxFrame(int minFrame, int maxFrame)
        {
            PlayMinAndMaxFrameCommand.ExecuteCommandIfPossible((minFrame, maxFrame));
        }

        public void PlayMinAndMaxProgress(float minProgress, float maxProgress)
        {
            PlayMinAndMaxProgressCommand.ExecuteCommandIfPossible((minProgress, maxProgress));
        }

        /// <summary>
        /// Reverses the current animation speed. This does NOT play the animation.
        /// </summary>
        public void ReverseAnimationSpeed()
        {
            ReverseAnimationSpeedCommand.ExecuteCommandIfPossible();
        }

        public void SetAnimationFromAssetOrBundle(string path)
        {
            AnimationSource = AnimationSource.AssetOrBundle;
            Animation = path;
        }

        public void SetAnimationFromEmbeddedResource(string resourceName, Assembly assembly = null)
        {
            AnimationSource = AnimationSource.EmbeddedResource;

            if (assembly == null)
                assembly = Xamarin.Forms.Application.Current.GetType().Assembly;

            Animation = $"resource://{resourceName}?assembly={Uri.EscapeUriString(assembly.FullName)}";
        }

        public void SetAnimationFromJson(string json)
        {
            AnimationSource = AnimationSource.Json;
            Animation = json;
        }

        public void SetAnimationFromUrl(string url)
        {
            AnimationSource = AnimationSource.Url;
            Animation = url;
        }

        public void SetAnimationFromStream(Stream stream)
        {
            AnimationSource = AnimationSource.Stream;
            Animation = stream;
        }

        // setImageAssetDelegate(ImageAssetDelegate assetDelegate) {

        // setFontAssetDelegate(

        // setTextDelegate(TextDelegate textDelegate)

        // setScaleType

        //RenderMode
    }
}
