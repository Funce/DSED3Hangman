using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Assessment3Hangman.Data
{
    class DataAdapter : BaseAdapter<tbl_highScore>
    {
        private readonly Activity context;

        private readonly List<tbl_highScore> items;

        public DataAdapter(Activity context, List<tbl_highScore> items)
        {
            this.context = context;
            this.items = items;
        }


        public override tbl_highScore this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            var view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);

            view.FindViewById<TextView>(Resource.Id.lblname).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.lblscore).Text = item.Score.ToString();
            return view;
        }
    }
}