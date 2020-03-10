using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
//using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace LottieSamples.Droid
{
    public class ChooseAssetDialogFragment : DialogFragment
    {
        private RecyclerView recyclerView;


        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_choose_asset, container, false);
            this.Dialog.SetTitle("Choose an Asset");

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            this.Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            var adapter = new AssetsAdapter(this.Activity);
            adapter.ItemClick += Adapter_ItemClick;
            this.recyclerView.SetAdapter(adapter);
        }

        private void Adapter_ItemClick(object sender, string fileName)
        {
            this.TargetFragment.OnActivityResult(TargetRequestCode,
                                                 (int)Android.App.Result.Ok,
                                                 new Intent().PutExtra(AnimationFragment.ExtraAnimationName, fileName));
            Dismiss();
        }


        private class AssetsAdapter : RecyclerView.Adapter
        {
            public event EventHandler<string> ItemClick;

            private IList<string> files = new List<string>();


            public AssetsAdapter(Context context)
            {
                files = AssetUtils.GetJsonAssets(context, string.Empty);
            }


            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_holder_file, parent, false);
                return new StringViewHolder(view, OnClick);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var vh = holder as StringViewHolder;

                var fileName = files[position];
                vh.Bind(fileName, fileName);
            }


            public override int ItemCount
            {
                get
                {
                    return files.Count;
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
