using System.Text.RegularExpressions;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LottieSamples.iOS
{
    public class LOTCharacterCell : UICollectionViewCell
    {

        private LOTAnimationView animationView;
        private string character;


        [Export("initWithFrame:")]
        public LOTCharacterCell(CGRect frame) : base(frame)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (this.animationView != null)
            {
                CGRect c = this.ContentView.Bounds;
                this.animationView.Frame = new CGRect(-c.Size.Width, 0, c.Size.Width * 3, c.Size.Height);
            }
        }

        public void SetCharacter(string character)
        {
            string sanatizedChar = character.Substring(0, 1);
            bool valid = Regex.IsMatch(sanatizedChar, @"^[a-z]+$", RegexOptions.IgnoreCase);


            if (character.Equals("BlinkingCursor"))
            {
                sanatizedChar = character;
            }

            if (sanatizedChar.Equals(","))
            {
                sanatizedChar = "Comma";
                valid = true;
            }
            else if (sanatizedChar.Equals("'"))
            {
                sanatizedChar = "Apostrophe";
                valid = true;
            }
            else if (sanatizedChar.Equals(":"))
            {
                sanatizedChar = "Colon";
                valid = true;
            }
            //else if (sanatizedChar.Equals("?"))
            //{
            //    sanatizedChar = "QuestionMark";
            //    valid = true;
            //}
            //else if (sanatizedChar.Equals("!"))
            //{
            //    sanatizedChar = "ExclamationMark";
            //    valid = true;
            //}
            //else if (sanatizedChar.Equals("."))
            //{
            //    sanatizedChar = "Period";
            //    valid = true;
            //}


            if (sanatizedChar.Equals(this.character))
            {
                return;
            }

            this.animationView?.RemoveFromSuperview();
            this.animationView = null;
            this.character = null;

            if (!valid)
                return;


            this.character = sanatizedChar;
            this.animationView = LOTAnimationView.AnimationNamed("TypeFace/" + sanatizedChar);
            this.animationView.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.ContentView.AddSubview(this.animationView);
            CGRect c = this.ContentView.Bounds;
            this.animationView.Frame = new CGRect(-c.Size.Width, 0, c.Size.Width * 3, c.Size.Height);
        }

        public void DisplayCharacter(bool animated)
        {
            if (this.animationView == null)
                return;

            if (animated)
            {
                this.animationView.Play();
            }
            else if (this.animationView.AnimationProgress != 1)
            {
                NSOperationQueue.MainQueue.AddOperation(() => this.animationView.AnimationProgress = 1);
            }
        }

        public void LoopAnimation()
        {
            this.animationView.LoopAnimation = true;
        }
    }
}
