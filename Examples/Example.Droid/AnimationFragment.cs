
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie.Model;
using Android.Animation;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using Uri = Android.Net.Uri;
using Android.Support.Design.Widget;
using Android.Database;
using Square.OkHttp3;
using Org.Json;

namespace LottieSamples.Droid
{
    public class AnimationFragment : Fragment, Animator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener
    {
        private const int RcAsset = 1337;
        private const int RcFile = 1338;
        private const int RcUrl = 1339;
        public const string ExtraAnimationName = "animation_name";

        private OkHttpClient client;

        private Toolbar toolbar;
        private ViewGroup instructionsContainer;
        private ViewGroup animationContainer;
        private LottieAnimationView animationView;
        private AppCompatSeekBar seekBar;
        private ImageButton invertButton;
        private ImageButton playButton;

        private ImageButton loopButton;
        private TextView animationNameView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_animation, container, false);


            this.toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.instructionsContainer = view.FindViewById<ViewGroup>(Resource.Id.instructions);
            this.animationContainer = view.FindViewById<ViewGroup>(Resource.Id.animation_container);
            this.animationView = view.FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            this.seekBar = view.FindViewById<AppCompatSeekBar>(Resource.Id.seek_bar);
            this.invertButton = view.FindViewById<ImageButton>(Resource.Id.invert_colors);
            this.playButton = view.FindViewById<ImageButton>(Resource.Id.play_button);

            this.loopButton = view.FindViewById<ImageButton>(Resource.Id.loop);
            this.animationNameView = view.FindViewById<TextView>(Resource.Id.animation_name);


            ImageButton restartButton = view.FindViewById<ImageButton>(Resource.Id.restart);
            ImageButton loadAssetButton = view.FindViewById<ImageButton>(Resource.Id.load_asset);
            ImageButton loadFieButton = view.FindViewById<ImageButton>(Resource.Id.load_file);
            ImageButton loadUrlButton = view.FindViewById<ImageButton>(Resource.Id.load_url);

            playButton.Click += PlayButton_Click;
            restartButton.Click += RestartButton_Click;
            loopButton.Click += LoopButton_Click;
            invertButton.Click += InvertButton_Click;
            loadAssetButton.Click += LoadAssetButton_Click;
            loadFieButton.Click += LoadFileButton_Click;
            loadUrlButton.Click += LoadUrlButton_Click;

            (this.Activity as AppCompatActivity).SetSupportActionBar(toolbar);
            toolbar.SetNavigationIcon(Resource.Drawable.ic_back);
            toolbar.NavigationClick += (sender, e) => FragmentManager.PopBackStack();


            PostUpdatePlayButtonText();
            LoopButton_Click(null, EventArgs.Empty);

            this.animationView.AddAnimatorListener(this);
            this.animationView.AddAnimatorUpdateListener(this);

            this.seekBar.ProgressChanged += (sender, e) => animationView.Progress = e.Progress/100f;

            return view;
        }

        public override void OnStop()
        {
            this.animationView.CancelAnimation();
            base.OnStop();
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode != (int)Android.App.Result.Ok)
                return;

            switch (requestCode)
            {

                case RcAsset:
                    string assetName = data.GetStringExtra(ExtraAnimationName);
                    LottieComposition.FromAssetFileName(this.Context, assetName, (composition) =>
                    {
                        SetComposition(composition, assetName);
                    });
                    break;

                case RcFile:
                    Uri uri = data.Data;
                    try
                    {
                        string path = GetPath(uri);

                    }
                    catch (Exception ex)
                    {
                        OnLoadError();
                    }
                    break;

                default:
                    break;
            }
        }

        void PlayButton_Click(object sender, EventArgs e)
        {
            if (animationView.IsAnimating)
            {
                animationView.PauseAnimation();
                PostUpdatePlayButtonText();
            }
            else 
            {
                if (animationView.Progress == 1f)
                {
                    animationView.Progress = 0f;
                }
                animationView.PlayAnimation();
                PostUpdatePlayButtonText();
            }
        }

        void RestartButton_Click(object sender, EventArgs e)
        {
            bool restart = animationView.IsAnimating;
            animationView.CancelAnimation();
            animationView.Progress = 0f;
            if (restart)
            {
                animationView.PlayAnimation();
            }
        }

        void LoopButton_Click(object sender, EventArgs e)
        {
            loopButton.Activated = !loopButton.Activated;
            animationView.Loop(loopButton.Enabled);
        }

        void InvertButton_Click(object sender, EventArgs e)
        {
            animationContainer.Activated = !animationContainer.Activated;
            invertButton.Activated = animationContainer.Activated;
        }

        private void LoadAssetButton_Click(object sender, EventArgs e)
        {
            animationView.CancelAnimation();
            DialogFragment assetFragment = new ChooseAssetDialogFragment();
            assetFragment.SetTargetFragment(this, RcAsset);
            assetFragment.Show(this.FragmentManager, "assets");
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            animationView.CancelAnimation();
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*.json");
            intent.AddCategory(Intent.CategoryOpenable);

            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select a JSON file"), RcFile);
            }
            catch (ActivityNotFoundException ex)
            {
                Toast.MakeText(this.Context, "Please install a File Manager.", ToastLength.Short).Show();
            }
        }


        private void LoadUrlButton_Click(object sender, EventArgs e)
        {
            animationView.CancelAnimation();
            EditText urlView = new EditText(this.Context);
            new AlertDialog.Builder(this.Context)
                           .SetTitle("Enter a URL")
                           .SetView(urlView)
                           .SetPositiveButton("Load", (dialog, ev) =>
                           {
                               LoadUrl(urlView.Text);

                           })
                           .SetNegativeButton("Cancel", (dialog, ev) =>
                           {
                ((AlertDialog)dialog).Dismiss();
                
                           })
                           .Show();
        }

        #region Animator.IAnimatorListener
        public void OnAnimationCancel(Animator animation)
        {
            PostUpdatePlayButtonText();
        }

        public void OnAnimationEnd(Animator animation)
        {
            PostUpdatePlayButtonText();
        }

        public void OnAnimationRepeat(Animator animation)
        {
            
        }

        public void OnAnimationStart(Animator animation)
        {
            
        }
        #endregion

        #region ValueAnimator.IAnimatorUpdateListener
        public void OnAnimationUpdate(ValueAnimator animation)
        {
            this.seekBar.Progress = (int)(animation.AnimatedFraction * 100);
        }
        #endregion


        private string GetPath(Uri uri)
        {
            if ("content".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                String[] projection = { "_data" };
                ICursor cursor = null;

                try
                {
                    cursor = Context.ContentResolver.Query(uri, projection, null, null, null);
                    int column_index = cursor.GetColumnIndexOrThrow("_data");
                    if (cursor.MoveToFirst())
                    {
                        return cursor.GetString(column_index);
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    if (cursor != null)
                    {
                        cursor.Close();
                    }
                }
            }
            else if ("file".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        private void LoadUrl(String url)
        {
            Request request;
            try
            {
                request = new Request.Builder()
                                     .Url(url)
                                     .Build();
            }
            catch (Exception ex)
            {
                OnLoadError();
                return;
            }


            if (this.client != null)
                this.client = new OkHttpClient();

            client.NewCall(request).Enqueue((ICall call, Response response) =>
            {
                if (!response.IsSuccessful)
                {
                    OnLoadError();
                }

                try
                {
                    JSONObject json = new JSONObject(response.Body().String());
                    LottieComposition.FromJson(this.Resources, json, (composition) =>
                    {
                        SetComposition(composition, "Network Animation");
                    });
                }
                catch(JSONException ex)
                {
                    OnLoadError();
                }

            },
                                            (ICall call, Java.IO.IOException e) => OnLoadError());
        }
    
        private void SetComposition(LottieComposition composition, String name)
        {
            instructionsContainer.Visibility = ViewStates.Gone;
            seekBar.Progress = 0;
            animationView.SetComposition(composition);
            animationNameView.Text = name;
        }
    
        private void PostUpdatePlayButtonText()
        {
            new Handler().Post(UpdatePlayButtonText);
        }

        private void UpdatePlayButtonText()
        {
            playButton.Activated = this.animationView.IsAnimating;
        }

        private void OnLoadError()
        {
            //noinspection ConstantConditions
            Snackbar.Make(this.View, "Failed to load animation", Snackbar.LengthLong).Show();
        }
    }
}
