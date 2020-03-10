using AppKit;
using Foundation;
using ObjCRuntime;

namespace Example.macOS
{
    public class EditMenuDelegate : NSMenuDelegate
    {
        public override void MenuWillHighlightItem(NSMenu menu, NSMenuItem item)
        {
        }

        public override void NeedsUpdate(NSMenu menu)
        {
            // Get list of menu items
            var Items = menu.ItemArray();

            foreach (var item in Items)
            {
                // Take action based on the menu title
                switch (item.Title)
                {
                    case "Paste":
                        // Only enable if there is an image on the pasteboard
                        item.Enabled = HasUrlOnPasteboard();
                        break;
                    default:
                        item.Enabled = item.HasSubmenu;
                        break;
                }
            }
        }

        private bool HasUrlOnPasteboard()
        {
            var pasteboard = NSPasteboard.GeneralPasteboard;
            var classArray = new[] { new Class(typeof(NSUrl)) };

            return pasteboard.CanReadObjectForClasses(classArray, null);
        }
    }
}