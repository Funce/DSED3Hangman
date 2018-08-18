using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Assessment3Hangman.Data;
using Android.Content;

namespace Assessment3Hangman
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button playButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            playButton = FindViewById<Button>(Resource.Id.btnplay);
            playButton.Click += PlayButton_Click;
            //if the DB is not there copy it to the Assets folder place
            try
            {
                DataManager.CopyTheDB();
            }
            catch (Exception e)
            {
                Log.Info("HECKIN", e.Message);
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Game))
                   .SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}

