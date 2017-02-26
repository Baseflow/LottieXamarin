using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LottieSamples.iOS
{
    public class AnimatedTextField : UIView, IUICollectionViewDataSource, IUICollectionViewDelegateFlowLayout
    {
        [Export("reuseIdentifier")]
        public static new string ReuseIdentifier => "char";

        private static NSString charToCursorSize = new NSString("W");

        private int fontSize;
        private string text = "";
        private UICollectionView collectionView;
        private UICollectionViewFlowLayout layout;
        private bool updatingCells;
        private IList<CGSize> letterSizes = new List<CGSize>();


        public UIEdgeInsets ScrollInsets 
        { 
            get 
            {
                return this.collectionView.ContentInset;
            }
            set
            {
                this.collectionView.ContentInset = value;
                this.ScrollToBottom();
            }
        }

        public int FontSize
        {
            get
            {
                return this.fontSize;
            }

            set
            {
                this.fontSize = value;
                ComputeLetterSizes();
                this.layout.InvalidateLayout();
            }
        }


        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                SetText(value);
            }
        }


        [Export("initWithFrame:")]
        public AnimatedTextField(CGRect frame) : base(frame)
        {
            this.layout = new UICollectionViewFlowLayout();
            this.fontSize = 36;
            this.collectionView = new UICollectionView(this.Bounds, this.layout);
            this.collectionView.RegisterClassForCell(typeof(LOTCharacterCell), ReuseIdentifier);
            this.collectionView.Delegate = this;
            this.collectionView.DataSource = this;
            this.collectionView.BackgroundColor = UIColor.White;
            this.AddSubview(this.collectionView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.collectionView.Frame = this.Bounds;
        }

        public void ScrollToBottom()
        {
            CGPoint bottomOffset = new CGPoint(0, this.collectionView.ContentSize.Height -
                                               this.collectionView.Bounds.Size.Height +
                                               this.collectionView.ContentInset.Bottom);
            bottomOffset.Y = (nfloat)Math.Max(bottomOffset.Y, 0);
            this.collectionView.SetContentOffset(bottomOffset, animated: true);
        }

        public void SetScrollInsets(UIEdgeInsets scrollInsets)
        {
            this.collectionView.ContentInset = scrollInsets;
            this.ScrollToBottom();
        }

        private void SetText(string newText)
        {
            this.text = newText;
            this.ComputeLetterSizes();
            this.collectionView.ReloadData();
            this.ScrollToBottom();
        }

        public void ChangeCharactersInRange(NSRange range, string toString)
        {
            StringBuilder newText = new StringBuilder(this.Text);
            if (range.Location > 0)
            {
                newText.Remove((int)range.Location, (int)range.Length);
                newText.Insert((int)range.Location, toString);
            }

            IList<NSIndexPath> updateIndices = null;
            IList<NSIndexPath> addIndices = null;
            IList<NSIndexPath> removeIndices = null;

            for (int i = (int)range.Location; i < newText.Length; i++)
            {
                if (i < this.text.Length)
                {
                    if (updateIndices == null)
                        updateIndices = new List<NSIndexPath>();

                    updateIndices.Add(NSIndexPath.FromRowSection(i, 0));
                }
                else
                {
                    if (addIndices == null)
                        addIndices = new List<NSIndexPath>();

                    addIndices.Add(NSIndexPath.FromRowSection(i, 0));
                }
            }

            for (int i = newText.Length; i < this.text.Length; i++)
            {
                if (removeIndices == null)
                    removeIndices = new List<NSIndexPath>();

                removeIndices.Add(NSIndexPath.FromRowSection(i, 0));
            }

            this.updatingCells = true;
            this.text = newText.ToString();
            ComputeLetterSizes();

            this.collectionView.PerformBatchUpdates(
                () =>
            {
                if (addIndices != null && addIndices.Count > 0)
                    this.collectionView.InsertItems(addIndices.ToArray());    

                if (updateIndices != null && updateIndices.Count > 0)
                    this.collectionView.ReloadItems(updateIndices.ToArray());

                if (removeIndices != null && removeIndices.Count > 0)
                    this.collectionView.DeleteItems(removeIndices.ToArray());

            }, 
                (finished) =>
            {
                updatingCells = false;
            });

            ScrollToBottom();
        }



        private string CharacterAtIndexPath(NSIndexPath indexPath)
        {
            return this.text.Substring(indexPath.Row, 1).ToUpper();
        }

        private void ComputeLetterSizes()
        {
            IList<CGSize> sizes = new List<CGSize>(this.text.Length);
            nfloat width = this.Bounds.Size.Width;
            nfloat currentWidth = 0;

            for (int i = 0; i < this.text.Length; i++)
            {
                string letter = this.text.Substring(i, 1).ToUpper();
                CGSize letterSize = SizeOfString(new NSString(letter));

                if (letter.Equals(" ") && i + 1 < this.text.Length)
                {

                    string cutString = this.text.Substring(i + 1);
                    string[] words = cutString.Split(' ');

                    if (words.Length > 0)
                    {
                        CGSize nextWordLength = SizeOfString(new NSString(words[0]));
                        if (currentWidth + nextWordLength.Width + letterSize.Width > width)
                        {
                            letterSize.Width = (nfloat) Math.Floor(width - currentWidth);
                            currentWidth = 0;
                        }
                        else
                        {
                            currentWidth += letterSize.Width;
                        }
                    }
                }
                else
                {
                    currentWidth += letterSize.Width;
                    if (currentWidth >= width)
                    {
                        currentWidth = letterSize.Width;
                    }
                }

                sizes.Add(letterSize);
            }

            CGSize cursorSize = SizeOfString(charToCursorSize);
            sizes.Add(cursorSize);


            this.letterSizes = sizes;
        }

        private CGSize SizeOfString(NSString text)
        {
            CGSize constraint = new CGSize(1000, 1000);
            CGSize textSize = text.GetBoundingRect(constraint,
                                                   options: NSStringDrawingOptions.UsesLineFragmentOrigin,
                                                   attributes: new UIStringAttributes { Font = UIFont.BoldSystemFontOfSize(this.FontSize) },
                                                   context: null).Size;

            textSize.Width += text.Length * 2;
            return textSize;
                                                   
        }

        #region UICollevtionViewDataSource
        public nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return this.text.Length + 1;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            LOTCharacterCell charCell = collectionView.DequeueReusableCell(ReuseIdentifier, indexPath) as LOTCharacterCell;
            return charCell;
        }
        #endregion

        #region IUICollectionViewDelegate
        [Export("collectionView:willDisplayCell:forItemAtIndexPath:")]
        public void WillDisplayCell(UICollectionView collectionView, LOTCharacterCell cell, NSIndexPath indexPath)
        {
            if (indexPath.Row < this.text.Length)
            {
                cell.SetCharacter(CharacterAtIndexPath(indexPath));
                cell.DisplayCharacter(animated: this.updatingCells);
            }
            else
            {
                cell.SetCharacter("BlinkingCursor");
                cell.LoopAnimation();
                cell.DisplayCharacter(animated:true);
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize SizeForItemAtIndexPath(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            if (indexPath.Row >= this.letterSizes.Count)
            {
                return CGSize.Empty;
            }

            return this.letterSizes[indexPath.Row];
        }

        [Export("collectionView:layout:minimumInteritemSpacingForSectionAtIndex:")]
        public Single GetMinimumInteritemSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, Int32 section)
        {
            return 0;
        }

        [Export("collectionView:layout:minimumLineSpacingForSectionAtIndex:")]
        public Single GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, Int32 section)
        {
            return 0;
        }
        #endregion
    }
}
