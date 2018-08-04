using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Assessment3Hangman.Data;

namespace Assessment3Hangman
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("Play");
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var itemTitle = item.TitleFormatted.ToString();

            switch (itemTitle)
            {
                case "Play":
                    StartActivity(typeof(Game));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}

