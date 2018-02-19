using System;
using Android.Content;
using Android.Widget;


using System.Collections.Generic;
using Android.Util;
using Android.Views;

using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie;
using Android.Views.InputMethods;

namespace LottieSamples.Droid
{
    public class LottieFontViewGroup : FrameLayout
    {
        private readonly IDictionary<string, LottieComposition> compositionMap = new Dictionary<string, LottieComposition>();
        private readonly IList<View> views = new List<View>();

        private LottieAnimationView cursorView;

        public LottieFontViewGroup(Context context) : this(context, null, 0)
        {
        }

        public LottieFontViewGroup(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public LottieFontViewGroup(Context context, IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
        {
            Init();
        }

        public override void AddView(View child, int index)
        {
            base.AddView(child, index);
            if (index == -1)
            {
                views.Add(child);
            }
            else 
            {
                views.Insert(index, child);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            if (views.Count == 0)
                return;

            int currentX = PaddingTop;
            int currenty = PaddingLeft;

            for (int i = 0; i < views.Count; i++)
            {
                View view = this.views[i];
                if (!FitsOnCurrentLine(currentX, view))
                {
                    if (view.Tag != null && view.Tag.ToString().Equals("Space"))
                    {
                        continue;
                    }

                    currentX = PaddingLeft;
                    currenty += view.MeasuredHeight;
                }

                currentX += view.Width;
            }

            SetMeasuredDimension(MeasuredWidth, currenty + views[views.Count - 1].MeasuredHeight * 2);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (views.Count == 0)
                return;

            int currentX = PaddingTop;
            int currentY = PaddingLeft;

            for (int i = 0; i < views.Count; i++)
            {
                View view = views[i];
                if (!FitsOnCurrentLine(currentX, view))
                {
                    if (view.Tag != null && view.Tag.ToString().Equals("Space"))
                    {
                        continue;
                    }
                    currentX = PaddingLeft;
                    currentY += view.MeasuredHeight;
                }

                view.Layout(currentX, currentY, currentX + view.MeasuredWidth, currentY + view.MeasuredHeight);
                currentX += view.Width;
            }
        }

        public override Android.Views.InputMethods.IInputConnection OnCreateInputConnection(Android.Views.InputMethods.EditorInfo outAttrs)
        {
            BaseInputConnection fic = new BaseInputConnection(this, false);
            outAttrs.ActionLabel = null;
            outAttrs.InputType = Android.Text.InputTypes.Null;
            outAttrs.ImeOptions = ImeFlags.NavigateNext;
            return fic;
        }

        public override bool OnCheckIsTextEditor()
        {
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Space)
            {
                AddSpace();
                return true;
            }

            if (keyCode == Keycode.Del)
            {
                RemoveLastView();
                return true;
            }

            if (!e.HasNoModifiers || !IsValidKey(keyCode))
                return base.OnKeyUp(keyCode, e);


            string letter = Char.ToUpper((char)e.UnicodeChar).ToString();
            string fileName = "Mobilo/" + letter + ".json";
            if (compositionMap.ContainsKey(fileName))
            {
                AddComposition(compositionMap[fileName]);
            }
            else
            {
                LottieComposition.Factory.FromAssetFileName(this.Context, fileName, (composition) =>
                {
                    if (!compositionMap.ContainsKey(fileName))
                    {
                        compositionMap.Add(fileName, composition);
                    }
                    AddComposition(composition);
                });
            }

            return true;
                
        }

        private void Init()
        {
            FocusableInTouchMode = true;
            LottieComposition.Factory.FromAssetFileName(this.Context, "Mobilo/BlinkingCursor.json", (composition) =>
            {
                cursorView = new LottieAnimationView(Context);
                cursorView.LayoutParameters = new LottieFontViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent
                );
                cursorView.Composition = composition;
                cursorView.Loop(true);
                cursorView.PlayAnimation();
                AddView(cursorView);
            });
        }

        private void AddSpace()
        {
            int index = IndexOfChild(cursorView);
            AddView(CreateSpaceView(), index);
        }

        private void RemoveLastView()
        {
            if (views.Count > 1)
            {
                int position = views.Count - 2;
                RemoveView(views[position]);
                views.RemoveAt(position);
            }
        }

        private bool IsValidKey(Keycode keyCode)
        {
            if (keyCode >= Keycode.A && keyCode <= Keycode.Z)
            {
                return true;
            }

            return false;
        }

        private void AddComposition(LottieComposition composition)
        {
            LottieAnimationView lottieAnimationView = new LottieAnimationView(this.Context);
            lottieAnimationView.LayoutParameters = new LottieFontViewGroup.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent
            );
            
            lottieAnimationView.Composition = composition;
            lottieAnimationView.PlayAnimation();
            if (cursorView == null)
            {
                AddView(lottieAnimationView);
            }
            else {
                int index = IndexOfChild(cursorView);
                AddView(lottieAnimationView, index);
            }
        }

        private bool FitsOnCurrentLine(int currentX, View view)
        {
            return currentX + view.MeasuredWidth < Width - PaddingRight;
        }

        private View CreateSpaceView()
        {
            View spaceView = new View(this.Context);
            spaceView.LayoutParameters = new LayoutParams(
                Resources.GetDimensionPixelSize(Resource.Dimension.font_space_width), 
                ViewGroup.LayoutParams.WrapContent
            );
            spaceView.Tag = "Space";
            return spaceView;
        }
    }
}
