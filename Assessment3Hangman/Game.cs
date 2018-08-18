using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Annotation;
using Android.Views;
using Android.Widget;
using static Android.App.ActionBar;

namespace Assessment3Hangman
{
    [Activity(Label = "Game")]
    public class Game : Activity
    {
        protected string WordToGuess;
        protected List<char> guessedLetters;
        protected int strikes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Game);

            strikes = 0;
            guessedLetters = new List<char>();

            create_word();
            FindViewById<Button>(Resource.Id.btnback).Click += btnback_Click; ;

            char[] row1 = "qwertyuiop".ToCharArray();
            char[] row2 = "asdfghjkl".ToCharArray();
            char[] row3 = "zxcvbnm".ToCharArray();
            char[][] rows = { row1, row2, row3 };
            LinearLayout keyboard1 = FindViewById<LinearLayout>(Resource.Id.linKeyboard1);
            LinearLayout keyboard2 = FindViewById<LinearLayout>(Resource.Id.linKeyboard2);
            LinearLayout keyboard3 = FindViewById<LinearLayout>(Resource.Id.linKeyboard3);
            LinearLayout[] keyboards = { keyboard1, keyboard2, keyboard3 };

            for (int row = 0; row < rows.Length; row++)
            {
                for (int letter = 0; letter < rows[row].Length; letter++)
                {
                    var newButton = new Button(this);
                    newButton.Click += onLetterPress;
                    newButton.Text = rows[row][letter].ToString();
                    newButton.SetBackgroundColor(new Android.Graphics.Color(255, 225, 30));
                    //newButton.SetForegroundGravity(GravityFlags.Center);
                    //newButton.SetPadding(50, 5, 50, 5);
                    var layoutParams = new LayoutParams(80, ViewGroup.LayoutParams.MatchParent);
                    layoutParams.SetMargins(10, 10, 10, 10);
                    keyboards[row].AddView(newButton, layoutParams);
                }
            }
            
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }

        private void onLetterPress(object sender, EventArgs e)
        {
            Button pressedButton = (Button)sender;
            string letter = pressedButton.Text;
            pressedButton.Clickable = false;
            pressedButton.SetBackgroundColor(new Android.Graphics.Color(180, 180, 180));
            guess_letter(letter);
        }

        protected void guess_letter(string letter)
        {
            //Lets cheat the type casting
            char guessedLetter = letter[0];
            bool found = false;
            var foundIndexes = new List<int>();
            for (int i = WordToGuess.IndexOf(guessedLetter); i > -1; i = WordToGuess.IndexOf(guessedLetter, i + 1))
            {
                found = true;
                // for loop end when i=-1 (guessedLetter not found)
                guessedLetters[i] = guessedLetter;
            }

            if (!found)
            {
                ++strikes;
            }
            //Check win conditions
            //Loss first
            /*
             *       |      | |
             *       | |    | _
             */
            //Nyehehe
            if (strikes == 7)
            {
                lose();
            }
            //THen the win conditions
            else if(guessedLetters.IndexOf('_') == -1)
            {
                win();
            }

            update_result();

        }

        protected void create_word()
        {
            WordToGuess = get_word();
            
            for (int i = 0; i < WordToGuess.Length; i++)
            {
                if (WordToGuess[i] == ' ')
                {
                    guessedLetters.Add('\n');
                }
                else
                {
                    guessedLetters.Add('_');
                }
                
            }
            Toast.MakeText(this, WordToGuess, ToastLength.Long).Show();

            update_result();
            

        }

        protected string get_word()
        {
            //Read in a txt file
            string word = "shipping container";
            return word;
        }

        protected void update_result()
        {
            var sb = new StringBuilder();
            foreach (var c in guessedLetters)
            {
                sb.Append(c);
            }
            //Change imageswitch to image[strikes]
            FindViewById<TextView>(Resource.Id.Strikes).Text = strikes.ToString();
            FindViewById<TextView>(Resource.Id.guessedLetters).Text = sb.ToString().ToUpper();
        }

        protected void win()
        {
            var dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Victory!");
            alert.SetMessage("Shipment Delivered!\n\nPlay again?");
            alert.SetButton("Yes", (c, ev) =>
            {
                Finish();
                var intent = new Intent(this, typeof(Game))
                    .SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(intent);
            });
            alert.SetButton2("Quit", (c, ev) => { });
            alert.Show();
        }

        protected void lose()
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Defeat");
            alert.SetMessage("You lost all the containers!");
            alert.SetButton("Restart", (c, ev) => { });
            alert.SetButton("Quit", (c, ev) => { });
            alert.Show();
        }
    }
}