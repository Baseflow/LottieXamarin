using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;
using static Android.Views.View;

namespace Lottie.Forms.Platforms.Android
{
    public class ClickListener : Java.Lang.Object, IOnClickListener
    {
        public ClickListener()
        {
        }

        public ClickListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public Action OnClickImpl { get; set; }

        public void OnClick(global::Android.Views.View v)
        {
            OnClickImpl?.Invoke();
        }
    }
}
