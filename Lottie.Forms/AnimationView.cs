using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Lottie.Forms
{
    public class AnimationView : View
    {
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress),
            typeof(float), typeof(AnimationView), default(float));

        public static readonly BindableProperty LoopProperty = BindableProperty.Create(nameof(Loop), typeof(bool),
            typeof(AnimationView), default(bool));

        public static readonly BindableProperty IsPlayingProperty = BindableProperty.Create(nameof(IsPlaying),
            typeof(bool), typeof(AnimationView), default(bool));

        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration),
            typeof(TimeSpan), typeof(AnimationView), default(TimeSpan));

        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation),
            typeof(string), typeof(AnimationView), default(string));

        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay),
            typeof(bool), typeof(AnimationView), default(bool));

        public float Progress
        {
            get { return (float) GetValue(ProgressProperty); }

            set { SetValue(ProgressProperty, value); }
        }

        public string Animation
        {
            get { return (string) GetValue(AnimationProperty); }

            set { SetValue(AnimationProperty, value); }
        }

        public TimeSpan Duration
        {
            get { return (TimeSpan) GetValue(DurationProperty); }

            set { SetValue(DurationProperty, value); }
        }

        public bool Loop
        {
            get { return (bool) GetValue(LoopProperty); }

            set { SetValue(LoopProperty, value); }
        }

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }

            set { SetValue(AutoPlayProperty, value); }
        }

        public bool IsPlaying
        {
            get { return (bool) GetValue(IsPlayingProperty); }

            set { SetValue(IsPlayingProperty, value); }
        }

        public event EventHandler OnPlay;

        public void Play()
        {
            OnPlay?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnPause;

        public void Pause()
        {
            OnPause?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnEnd;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void FireOnEnd()
        {
            OnEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}