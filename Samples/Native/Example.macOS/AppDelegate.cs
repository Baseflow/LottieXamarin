using AppKit;
using Foundation;

namespace Example.macOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Disable automatice item enabling on the Edit menu
            editMenu.AutoEnablesItems = false;
            editMenu.Delegate = new EditMenuDelegate();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
