using System;

namespace Lottie.Forms
{
    public class SegmentEventArgs : EventArgs
    {
        public float From { get; set; }
        public float To { get; set; }

        public SegmentEventArgs()
            :base()
        {
        }

        public SegmentEventArgs(float from, float to)
        {
            From = from;
            To = to;
        }
    }
}
