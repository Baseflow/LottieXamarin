
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace LottieSamples.Droid
{
    [Activity(Label = "FontActivity")]
    public class FontActivity : AppCompatActivity, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private ScrollView scrollView;
        private LottieFontViewGroup fontView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_font);


            this.scrollView = FindViewById<ScrollView>(Resource.Id.scroll_view);
            this.fontView = FindViewById<LottieFontViewGroup>(Resource.Id.font_view);

            this.fontView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

        protected override void OnDestroy()
        {
            fontView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            base.OnDestroy();
        }

        public void OnGlobalLayout()
        {
            scrollView.FullScroll(FocusSearchDirection.Down);
        }
    }
}
