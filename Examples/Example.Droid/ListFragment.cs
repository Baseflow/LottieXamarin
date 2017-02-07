
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;

namespace LottieSamples.Droid
{
    public class ListFragment : Fragment
    {
        private RecyclerView recyclerView;
        private LottieAnimationView animationView;

        private readonly FileAdapter adapter = new FileAdapter();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_list, container, false);

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
            this.animationView = view.FindViewById<LottieAnimationView>(Resource.Id.animation_view);

            this.recyclerView.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            this.animationView.Progress = 0f;
            this.animationView.PlayAnimation();
        }

        public override void OnStop()
        {
            base.OnStop();
            this.animationView.CancelAnimation();
        }

        void Adapter_ItemClick(object sender, string e)
        {
            if (FileAdapter.TagViewer.Equals(e))
            {
                ShowFragment(new AnimationFragment());
            }
            else if (FileAdapter.TagTypography.Equals(e))
            {
                this.StartActivity(new Intent(this.Context, typeof(FontActivity)));
            }
        }

        private void ShowFragment(Fragment fragment)
        {
                FragmentManager.BeginTransaction()
                               .AddToBackStack(null)
                               .SetCustomAnimations(Resource.Animation.slide_in_right, Resource.Animation.hold, Resource.Animation.hold, Resource.Animation.slide_out_right)
                               .Remove(this)
                               .Replace(Resource.Id.content_2, fragment)
                               .Commit();
        }




        private class FileAdapter : RecyclerView.Adapter
        {
            public const string TagViewer = "viewer";
            public const string TagTypography = "typography";
            public const string TagAppIntro = "app_intro";

            public event EventHandler<string> ItemClick;

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_holder_file, parent, false);
                return new StringViewHolder(view, OnClick);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {

                StringViewHolder vh = holder as StringViewHolder;

                switch (position)
                {
                    case 0:
                        vh.Bind("Animation Viewer", TagViewer);
                        break;
                    case 1:
                        vh.Bind("Animated Typography", TagTypography);
                        break;
                    case 2:
                        vh.Bind("Animated App Tutorial", TagAppIntro);
                        break;
                    default:
                        break;
                }
            }

            public override int ItemCount
            {
                get
                {
                    return 2;
                }
            }

            private void OnClick(string tag)
            {
                if (ItemClick != null)
                    ItemClick(this, tag);
            }
        }
    }
}
