using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace LottieSamples.iOS
{
    public partial class LottieRootViewController : UIViewController
    {
        private LOTAnimationView lottieLogo;
        private UIButton lottieButton;
        private UITableView tableView;

        protected LottieRootViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.lottieLogo = LOTAnimationView.AnimationNamed("LottieLogo1");
            this.lottieLogo.ContentMode = UIViewContentMode.ScaleAspectFill;
            this.View.AddSubview(this.lottieLogo);

            this.lottieButton = new UIButton(UIButtonType.Custom);
            this.lottieButton.AddTarget((sender, e) => { PlayLottieAnimation(); }, UIControlEvent.TouchUpInside);
            this.View.AddSubview(lottieButton);

            this.tableView = new UITableView(CGRect.Empty, UITableViewStyle.Plain);

            TableSource source = new TableSource();
            source.ItemSelected += Source_ItemSelected;
            this.tableView.Source = source;
            this.View.Add(this.tableView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.lottieLogo.Play();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.lottieLogo.Pause();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            CGRect lottieRect = new CGRect(0, 0, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height * 0.3);
            this.lottieLogo.Frame = lottieRect;
            this.lottieButton.Frame = lottieRect;

            this.tableView.Frame = new CGRect(0, lottieRect.GetMaxY(), lottieRect.Width, this.View.Bounds.Size.Height - lottieRect.GetMaxY());
        }



        private void PlayLottieAnimation()
        {
            this.lottieLogo.AnimationProgress = 0;
            this.lottieLogo.Play();
        }

        private void Source_ItemSelected(object sender, Type classType)
        {
            UIViewController vc = (UIViewController)Activator.CreateInstance(classType);
            this.PresentViewController(vc, animated: true, completionHandler: null);
        }



        private class TableSource : UITableViewSource
        {
            public event EventHandler<Type> ItemSelected;

            private const string CellIdentifier = "Cell";
            private readonly IList<Tuple<string, Type>> listItems = new List<Tuple<string, Type>>(4);


            public TableSource()
            {
                this.listItems.Add(new Tuple<string, Type>("Animation Explorer", typeof(AnimationExplorerViewController)));
                this.listItems.Add(new Tuple<string, Type>("Animated Keyboard", typeof(TypingDemoViewController)));
                this.listItems.Add(new Tuple<string, Type>("Animated Transitions Demo", typeof(AnimationTransitionViewController)));
                this.listItems.Add(new Tuple<string, Type>("Animated UIControls Demo", typeof(LAControlsViewController)));
            }


            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return this.listItems.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                }

                cell.TextLabel.Text = this.listItems[indexPath.Row].Item1;

                return cell;

            }

            public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                return 50f;
            }

            public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                this.ItemSelected?.Invoke(null, this.listItems[indexPath.Row].Item2);
                tableView.DeselectRow(indexPath, animated: true);
            }
        }

    }
}
