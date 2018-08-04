using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Assessment3Hangman.Data
{
    public static class DataManager
    {
        public static SQLiteConnection db;
        public static string databasePath;
        public static string databaseName;
        static DataManager()
        {//Set the DB connection string
            databaseName = "hangman.db";
            databasePath =
           Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(),
           databaseName);
            db = new SQLiteConnection(databasePath);
        }
        public static List<tbl_highScore> ViewAll()
        {
            try
            {
                return db.Query<tbl_highScore>("SELECT * FROM tbl_highScore");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                //making some fake items to stop the system from crashing when the DB doesn't connect
                var fakeitem = new List<tbl_highScore>();
                //make a single item
                var item = new tbl_highScore
                {
                    Id = 100,
                    Score = -3,
                    Name = "Error Reading"
                };
                fakeitem.AddRange(new[] { item }); //add it to the fake item list
                return fakeitem;
            }
        }
        public static void AddItem(string name, int score)
        {
            try
            {
                var addThis = new tbl_highScore() { Name = name, Score = score };
                db.Insert(addThis);
            }
            catch (Exception e)
            {
                Console.WriteLine("Add Error:" + e.Message);
            }
        }
        public static void EditItem(string name, int score, int listid)
        {
            try
            {
                // http://stackoverflow.com/questions/14007891/how-are-sqlite-records-updated
                var EditThis = new tbl_highScore()
                {
                    Name = name,
                    Score = score,
                    Id = listid
                };
                db.Update(EditThis);
            }
            catch (Exception e)
            {
                Console.WriteLine("Update Error:" + e.Message);
            }
        }
        public static void DeleteItem(int listid)
        {
            // https://developer.xamarin.com/guides/crossplatform/application_fundamentals/data/part_3_using_sqlite_orm/

            try
            {
                db.Delete<tbl_highScore>(listid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Error:" + ex.Message);
            }
        }
        public static void CopyTheDB(bool force = true)
        {
            /* Check if your DB has already been extracted. If the file does not exist
            move it.
            //WARNING!!!!!!!!!!! If your DB changes from the first time you install
            it, ie: you change the tables, and you get errors then comment out the if
            wrapper so that it is FORCED TO UPDATE.
            //Otherwise you spend hours staring at code that should run OK but the db,
            that you can’t see inside of on your phone, is diffferent from the db you
            are coding to.*/
            if (!File.Exists(DataManager.databasePath) || force)
            {
                AssetManager Assets = Android.App.Application.Context.Assets;
                using (BinaryReader br = new
               BinaryReader(Assets.Open(DataManager.databaseName)))
                {
                    using (BinaryWriter bw = new BinaryWriter(new
                   FileStream(DataManager.databasePath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int len = 0;
                        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, len);
                        }
                    }
                }
            }
        }
    }
}