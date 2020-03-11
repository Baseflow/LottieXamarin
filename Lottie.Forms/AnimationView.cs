using System;
using System.ComponentModel;
using System.Windows.Input;
using Lottie.Forms.EventArguments;
using Xamarin.Forms;

namespace Lottie.Forms
{
    public class AnimationView : View
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image),
            typeof(ImageSource), typeof(AnimationView), default(ImageSource));

        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation),
            typeof(string), typeof(AnimationView), default(string));

        public static readonly BindableProperty CacheCompositionProperty = BindableProperty.Create(nameof(CacheComposition),
            typeof(bool), typeof(AnimationView), default(bool));

        public static readonly BindableProperty FallbackResourceProperty = BindableProperty.Create(nameof(FallbackResource),
            typeof(ImageSource), typeof(AnimationView), default(ImageSource));

        public static readonly BindableProperty CompositionProperty = BindableProperty.Create(nameof(Composition),
            typeof(object), typeof(AnimationView), default(object));

        public static readonly BindableProperty MinFrameProperty = BindableProperty.Create(nameof(MinFrame),
            typeof(int), typeof(AnimationView), default(int));

        public static readonly BindableProperty MinProgressProperty = BindableProperty.Create(nameof(MinProgress),
            typeof(float), typeof(AnimationView), default(float));
        
        public static readonly BindableProperty MaxFrameProperty = BindableProperty.Create(nameof(MaxFrame),
            typeof(int), typeof(AnimationView), default(int));
        
        public static readonly BindableProperty MaxProgressProperty = BindableProperty.Create(nameof(MaxProgress),
            typeof(float), typeof(AnimationView), default(float));

        public static readonly BindableProperty SpeedProperty = BindableProperty.Create(nameof(Speed),
            typeof(float), typeof(AnimationView), 1.0f);

        public static readonly BindableProperty RepeatModeProperty = BindableProperty.Create(nameof(RepeatMode),
            typeof(RepeatMode), typeof(AnimationView), default(RepeatMode));

        public static readonly BindableProperty RepeatCountProperty = BindableProperty.Create(nameof(RepeatCount),
            typeof(int), typeof(AnimationView), default(int));

        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating),
            typeof(bool), typeof(AnimationView), default(bool));

        public static readonly BindableProperty ImageAssetsFolderProperty = BindableProperty.Create(nameof(ImageAssetsFolder),
            typeof(string), typeof(AnimationView), default(string));

        public static new readonly BindableProperty ScaleProperty = BindableProperty.Create(nameof(Scale),
        typeof(float), typeof(AnimationView), 1.0f);

        public static readonly BindableProperty FrameProperty = BindableProperty.Create(nameof(Frame),
        typeof(int), typeof(AnimationView), default(int));

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress),
        typeof(float), typeof(AnimationView), 1.0f);

        //TODO: Maybe make TimeSpan
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration),
        typeof(long), typeof(AnimationView), default(long));

        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay),
            typeof(bool), typeof(AnimationView), true);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(AnimationView));

        public static readonly BindableProperty PlayAnimationCommandProperty = BindableProperty.Create(nameof(PlayAnimationCommand),
            typeof(ICommand), typeof(AnimationView));

        // void setMinAndMaxFrame(int minFrame, int maxFrame)

        // void setMinAndMaxProgress(float minProgress, float maxProgress)

        // void reverseAnimationSpeed

        // Bitmap updateBitmap(String id, @Nullable Bitmap bitmap)

        // setImageAssetDelegate(ImageAssetDelegate assetDelegate) {

        // setFontAssetDelegate(

        // setTextDelegate(TextDelegate textDelegate)

        //setScaleType

        public long Duration
        {
            get { return (long)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public bool CacheComposition
        {
            get { return (bool)GetValue(CacheCompositionProperty); }
            set { SetValue(CacheCompositionProperty, value); }
        }

        public string Animation
        {
            get { return (string)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public ImageSource FallbackResource
        {
            get { return (ImageSource)GetValue(FallbackResourceProperty); }
            set { SetValue(FallbackResourceProperty, value); }
        }

        public object Composition
        {
            get { return (object)GetValue(CompositionProperty); }
            set { SetValue(CompositionProperty, value); }
        }

        public int MinFrame
        {
            get { return (int)GetValue(MinFrameProperty); }
            set { SetValue(MinFrameProperty, value); }
        }

        public float MinProgress
        {
            get { return (float)GetValue(MinProgressProperty); }
            set { SetValue(MinProgressProperty, value); }
        }

        public int MaxFrame
        {
            get { return (int)GetValue(MaxFrameProperty); }
            set { SetValue(MaxFrameProperty, value); }
        }

        public float MaxProgress
        {
            get { return (float)GetValue(MaxProgressProperty); }
            set { SetValue(MaxProgressProperty, value); }
        }

        public float Speed
        {
            get { return (float)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        public RepeatMode RepeatMode
        {
            get { return (RepeatMode)GetValue(RepeatModeProperty); }
            set { SetValue(RepeatModeProperty, value); }
        }

        public int RepeatCount
        {
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public string ImageAssetsFolder
        {
            get { return (string)GetValue(ImageAssetsFolderProperty); }
            set { SetValue(ImageAssetsFolderProperty, value); }
        }

        public new float Scale
        {
            get { return (float)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public int Frame
        {
            get { return (int)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public ICommand PlayAnimationCommand
        {
            get { return (ICommand)GetValue(PlayAnimationCommandProperty); }
            set { SetValue(PlayAnimationCommandProperty, value); }
        }

        public void PlayAnimation()
        {
            NativePlayCommand.ExecuteCommandIfPossible();
        }

        public void ResumeAnimation()
        {
            NativeResumeCommand.ExecuteCommandIfPossible();
        }

        public void CancelAnimation()
        {
            NativeCancelCommand.ExecuteCommandIfPossible();
        }

        public void PauseAnimation()
        {
            NativePauseCommand.ExecuteCommandIfPossible();
        }

        internal ICommand NativePlayCommand { get; set; }
        internal ICommand NativeResumeCommand { get; set; }
        internal ICommand NativeCancelCommand { get; set; }
        internal ICommand NativePauseCommand { get; set; }

        internal void InvokePlayAnimation()
        {
            OnPlayAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeResumeAnimation()
        {
            OnResumeAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeCancelAnimation()
        {
            OnCancelAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokePauseAnimation()
        {
            OnPauseAnimation?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeRepeatAnimation()
        {
            OnRepeatAnimation?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnPlayAnimation;

        public event EventHandler OnResumeAnimation;

        public event EventHandler OnCancelAnimation;

        public event EventHandler OnPauseAnimation;

        public event EventHandler OnRepeatAnimation;

        internal void InvokeAnimatorUpdate()
        {
            OnAnimatorUpdate?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeAnimator()
        {
            OnAnimator?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeCompositionLoaded(object composition)
        {
            //TODO: use composition
            OnCompositionLoaded?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeFailure(Exception ex)
        {
            OnFailure?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnAnimatorUpdate;

        public event EventHandler OnAnimator;

        public event EventHandler OnCompositionLoaded;

        public event EventHandler OnFailure;

        public void Click()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
            Command.ExecuteCommandIfPossible();
        }

        public event EventHandler OnClick;

        internal void InvokePlaybackEnded()
        {
            OnEnded?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnEnded;

        //PerformanceTrackingEnabled

        //RenderMode

        //ApplyingOpacityToLayersEnabled

        //disableExtraScaleModeInFitXY

        /*

        public static readonly BindableProperty LoopProperty = BindableProperty.Create(nameof(Loop), typeof(bool),
            typeof(AnimationView), default(bool));

        public static readonly BindableProperty IsPlayingProperty = BindableProperty.Create(nameof(IsPlaying),
            typeof(bool), typeof(AnimationView), default(bool));

        public static readonly BindableProperty PlaybackStartedCommandProperty = BindableProperty.Create(nameof(PlaybackStartedCommand),
            typeof(ICommand), typeof(AnimationView));

        public static readonly BindableProperty PlaybackFinishedCommandProperty = BindableProperty.Create(nameof(PlaybackFinishedCommand),
            typeof(ICommand), typeof(AnimationView));

        

        public static readonly BindableProperty HardwareAccelerationProperty = BindableProperty.Create(nameof(HardwareAcceleration),
            typeof(bool), typeof(AnimationView), default(bool));

        public bool Loop
        {
            get { return (bool)GetValue(LoopProperty); }
            set { SetValue(LoopProperty, value); }
        }

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public ICommand PlaybackStartedCommand
        {
            get { return (ICommand)GetValue(PlaybackStartedCommandProperty); }
            set { SetValue(PlaybackStartedCommandProperty, value); }
        }

        public ICommand PlaybackFinishedCommand
        {
            get { return (ICommand)GetValue(PlaybackFinishedCommandProperty); }
            set { SetValue(PlaybackFinishedCommandProperty, value); }
        }

        public ICommand ClickedCommand
        {
            get { return (ICommand)GetValue(ClickedCommandProperty); }
            set { SetValue(ClickedCommandProperty, value); }
        }

        /// <summary>
        /// Where possible/supported render the animation using hardware (GPU) rather than software (CPU) More information: https://airbnb.io/lottie/android/performance.html#hardware-acceleration
        /// No Effect on iOS
        /// </summary>
        public bool HardwareAcceleration
        {
            get { return (bool)GetValue(HardwareAccelerationProperty); }
            set { SetValue(HardwareAccelerationProperty, value); }
        }

        public event EventHandler OnPlay;

        public void Play()
        {
            OnPlay?.Invoke(this, new EventArgs());
            ExecuteCommandIfPossible(PlaybackStartedCommand);
        }

        public event EventHandler<ProgressSegmentEventArgs> OnPlayProgressSegment;

        public void PlayProgressSegment(float from, float to)
        {
            if (from < 0f || from > 1f)
                throw new ArgumentException($"Parameter {nameof(from)} should have a valid value.", nameof(from));

            if (to < 0f || to > 1f)
                throw new ArgumentException($"Parameter {nameof(to)} should have a valid value.", nameof(to));

            OnPlayProgressSegment?.Invoke(this, new ProgressSegmentEventArgs(from, to));

            ExecuteCommandIfPossible(PlaybackStartedCommand);
        }

        public event EventHandler<FrameSegmentEventArgs> OnPlayFrameSegment;

        public void PlayFrameSegment(int from, int to)
        {
            if (from < 0)
                throw new ArgumentException($"Parameter {nameof(from)} should have a valid value.", nameof(from));

            if (to < 0)
                throw new ArgumentException($"Parameter {nameof(to)} should have a valid value.", nameof(to));

            OnPlayFrameSegment?.Invoke(this, new FrameSegmentEventArgs(from, to));

            ExecuteCommandIfPossible(PlaybackStartedCommand);
        }

        public event EventHandler OnPause;

        public void Pause()
        {
            OnPause?.Invoke(this, new EventArgs());
        }

        

        

        */
    }
}
