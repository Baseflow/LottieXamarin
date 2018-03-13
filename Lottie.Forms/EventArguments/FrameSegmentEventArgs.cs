using System;

namespace Lottie.Forms.EventArguments
{
    public class FrameSegmentEventArgs : EventArgs
    {
        public int From { get; set; }
        public int To { get; set; }

        public FrameSegmentEventArgs()
            :base()
        {
        }

        public FrameSegmentEventArgs(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}
