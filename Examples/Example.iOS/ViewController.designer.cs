// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AnimationView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider Slider { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AnimationView != null) {
                AnimationView.Dispose ();
                AnimationView = null;
            }

            if (Slider != null) {
                Slider.Dispose ();
                Slider = null;
            }
        }
    }
}