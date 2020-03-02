using Android.App;
using Android.Widget;
using Android.OS;
using AndroidX.AppCompat.App;

namespace LottieSamples.Droid
{
    [Activity(Label = "Lottie Samples", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            if (savedInstanceState == null)
            {
                this.SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.content_1, new ListFragment())
                    .Commit();
            }
        }
    }
}