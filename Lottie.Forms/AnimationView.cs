﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Lottie.Forms.EventArguments;
using Xamarin.Forms;

namespace Lottie.Forms
{
    public class AnimationView : View, IElementConfiguration<AnimationView>
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

        [Obsolete("AutoPlay is obsolete, use IsPlaying instead")]
        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay),
            typeof(bool), typeof(AnimationView), default(bool));

        public static readonly BindableProperty SpeedProperty = BindableProperty.Create(nameof(Speed),
            typeof(float), typeof(AnimationView), 1.0f);

        public static readonly BindableProperty PlaybackStartedCommandProperty = BindableProperty.Create(nameof(PlaybackStartedCommand),
            typeof(ICommand), typeof(AnimationView));

        public static readonly BindableProperty PlaybackFinishedCommandProperty = BindableProperty.Create(nameof(PlaybackFinishedCommand),
            typeof(ICommand), typeof(AnimationView));

        public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(nameof(ClickedCommand),
            typeof(ICommand), typeof(AnimationView));

        public static readonly BindableProperty ImageAssetsFolderProperty = BindableProperty.Create(nameof(ImageAssetsFolder),
            typeof(string), typeof(AnimationView), default(string));

        public static readonly BindableProperty HardwareAccelerationProperty = BindableProperty.Create(nameof(HardwareAcceleration),
            typeof(bool), typeof(AnimationView), default(bool));

        readonly Lazy<Dictionary<Type, object>> _platformSpecifics;

        public AnimationView()
        {
            _platformSpecifics = new Lazy<Dictionary<Type, object>>();
        }

        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public string Animation
        {
            get { return (string)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public bool Loop
        {
            get { return (bool)GetValue(LoopProperty); }
            set { SetValue(LoopProperty, value); }
        }

        [Obsolete("AutoPlay is obsolete, use IsPlaying instead")]
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public float Speed
        {
            get { return (float)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
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

        public string ImageAssetsFolder
        {
            get { return (string)GetValue(ImageAssetsFolderProperty); }
            set { SetValue(ImageAssetsFolderProperty, value); }
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

        public event EventHandler OnClick;

        public void Click()
        {
            OnClick?.Invoke(this, new EventArgs());

            ExecuteCommandIfPossible(ClickedCommand);
        }

        public event EventHandler OnFinish;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void PlaybackFinished()
        {
            OnFinish?.Invoke(this, EventArgs.Empty);

            ExecuteCommandIfPossible(PlaybackFinishedCommand);
        }

        private void ExecuteCommandIfPossible(ICommand command)
        {
            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        public IPlatformElementConfiguration<T, AnimationView> On<T>() where T : IConfigPlatform
        {
            if (_platformSpecifics.Value.ContainsKey(typeof(T)))
                return (IPlatformElementConfiguration<T, AnimationView>)_platformSpecifics.Value[typeof(T)];

            var emptyConfig = Configuration<T, AnimationView>.Create(this);
            _platformSpecifics.Value.Add(typeof(T), emptyConfig);
            return emptyConfig;
        }
    }
}
