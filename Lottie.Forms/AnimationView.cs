using System;
using System.ComponentModel;
using System.Windows.Input;
using Lottie.Forms.EventArguments;
using Xamarin.Forms;

namespace Lottie.Forms
{
    public class AnimationView : View
    {
        //public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image),
        //    typeof(ImageSource), typeof(AnimationView), default(ImageSource));

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

        public long Duration
        {
            get { return (long)GetValue(ProgressProperty); }
            internal set { SetValue(ProgressProperty, value); }
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
            internal set { SetValue(IsAnimatingProperty, value); }
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
            Clicked?.Invoke(this, EventArgs.Empty);
            Command.ExecuteCommandIfPossible();
        }

        public event EventHandler Clicked;

        internal void InvokePlaybackEnded()
        {
            OnEnded?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnEnded;

        public void PlayAnimation()
        {
            PlayCommand.ExecuteCommandIfPossible();
        }

        public void ResumeAnimation()
        {
            ResumeCommand.ExecuteCommandIfPossible();
        }

        public void CancelAnimation()
        {
            CancelCommand.ExecuteCommandIfPossible();
        }

        public void PauseAnimation()
        {
            PauseCommand.ExecuteCommandIfPossible();
        }

        public void SetMinAndMaxFrame(int minFrame, int maxFrame)
        {
            SetMinAndMaxFrameCommand.ExecuteCommandIfPossible((minFrame, maxFrame));
        }

        public void SetMinAndMaxProgress(float minProgress, float maxProgress)
        {
            SetMinAndMaxProgressCommand.ExecuteCommandIfPossible((minProgress, maxProgress));
        }
        public void ReverseAnimationSpeed()
        {
            ReverseAnimationSpeedCommand.ExecuteCommandIfPossible();
        }

        internal ICommand PlayCommand { get; set; }
        internal ICommand ResumeCommand { get; set; }
        internal ICommand CancelCommand { get; set; }
        internal ICommand PauseCommand { get; set; }
        internal ICommand SetMinAndMaxFrameCommand { get; set; }
        internal ICommand SetMinAndMaxProgressCommand { get; set; }
        internal ICommand ReverseAnimationSpeedCommand { get; set; }

        // Bitmap updateBitmap(String id, @Nullable Bitmap bitmap)

        // setImageAssetDelegate(ImageAssetDelegate assetDelegate) {

        // setFontAssetDelegate(

        // setTextDelegate(TextDelegate textDelegate)

        // setScaleType

        //PerformanceTrackingEnabled

        //RenderMode

        //ApplyingOpacityToLayersEnabled

        //disableExtraScaleModeInFitXY

       
            /*
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

        */
    }
}
