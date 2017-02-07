
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Airbnb.Lottie;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace LottieSamples.Droid
{
    public class ViewAnimationFragment : Fragment
    {

        private View messageBubble;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_view_animation, container, false);

            this.messageBubble = view.FindViewById<View>(Resource.Id.message_bubble);
            this.messageBubble.SetTag(Resource.Id.lottie_layer_name, "Tip");
            LottieViewAnimator.Of(this.Context, "Tip.json", messageBubble)
                              .Loop(true)
                              .Start();
            return view;
        }
    }
}
