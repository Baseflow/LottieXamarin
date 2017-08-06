// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Example.macOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton loopButton { get; set; }

		[Outlet]
		AppKit.NSButton playButton { get; set; }

		[Outlet]
		AppKit.NSSlider progressSlider { get; set; }

		[Outlet]
		AppKit.NSButton rewindButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (progressSlider != null) {
				progressSlider.Dispose ();
				progressSlider = null;
			}

			if (rewindButton != null) {
				rewindButton.Dispose ();
				rewindButton = null;
			}

			if (playButton != null) {
				playButton.Dispose ();
				playButton = null;
			}

			if (loopButton != null) {
				loopButton.Dispose ();
				loopButton = null;
			}
		}
	}
}
