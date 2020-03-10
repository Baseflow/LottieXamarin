using System;

namespace Lottie.Forms.EventArguments
{
    public class ProgressSegmentEventArgs : EventArgs
    {
        public float From { get; set; }
        public float To { get; set; }

        public ProgressSegmentEventArgs()
            : base()
        {
        }

        public ProgressSegmentEventArgs(float from, float to)
        {
            From = from;
            To = to;
        }
    }
}
