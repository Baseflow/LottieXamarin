using System;

using UIKit;

namespace LottieSamples.iOS
{
    public partial class JSONExplorerViewController : UIViewController
    {

        public Action<string> CompletionBlock;

        private UITableView tableView;
        private string[] jsonFiles;

        public JSONExplorerViewController() : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.White;

            this.jsonFiles = Foundation.NSBundle.MainBundle.PathsForResources("json");


            this.tableView = new UITableView(this.View.Bounds);
            this.tableView.RegisterClassForCellReuse(typeof(UITableViewCell), "cell");

            var source = new TableSource(this.jsonFiles);
            source.ItemSelected += Source_ItemSelected;
            this.tableView.Source = source;    

            this.View.AddSubview(this.tableView);

            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Close",
                                                                        UIBarButtonItemStyle.Done,
                                                                        (sender, e) => { ClosePressed(); });
        }


        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            this.tableView.Frame = this.View.Bounds;
        }

        private void ClosePressed()
        {
            if (this.CompletionBlock != null)
            {
                this.CompletionBlock(null);
            }
        }

        private void Source_ItemSelected(object sender, string fileName)
        {
            if (this.CompletionBlock != null)
            {
                this.CompletionBlock(fileName);
            }
        }


        private class TableSource : UITableViewSource
        {
            public event EventHandler<string> ItemSelected;

            private const string CellIdentifier = "Cell";
            private readonly string[] jsonFiles;

            public TableSource(string[] jsonFiles)
            {
                this.jsonFiles = jsonFiles;
            }


            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return this.jsonFiles.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell("cell");

                //if (cell == null)
                //{
                //    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                //}
                cell.TextLabel.Text = System.IO.Path.GetFileName(this.jsonFiles[indexPath.Row]);
                return cell;

            }

            public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                this.ItemSelected?.Invoke(null, System.IO.Path.GetFileName(this.jsonFiles[indexPath.Row]));
                tableView.DeselectRow(indexPath, animated: true);
            }
        }
       
    }
}

