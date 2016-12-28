using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using EntityFrameworkWithXamarin.Core;
using System.Collections.Generic;

namespace EntityFrameworkWithXamarin.Droid
{
    [Activity(Label = "EntityFrameworkWithXamarin.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            TextView textView = FindViewById<TextView>(Resource.Id.TextView1);

            var dbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var fileName = "Cats.db";
            var dbFullPath = Path.Combine(dbFolder, fileName);
            try
            {
                using (var db = new CatContext(dbFullPath))
                {
                    await db.Database.MigrateAsync(); //We need to ensure the latest Migration was added. This is different than EnsureDatabaseCreated.

                    Cat catGary = new Cat() { CatId = 1, Name = "Gary", MeowsPerSecond = 5 };
                    Cat catJack = new Cat() { CatId = 2, Name = "Jack", MeowsPerSecond = 11 };
                    Cat catLuna = new Cat() { CatId = 3, Name = "Luna", MeowsPerSecond = 3 };

                    List<Cat> catsInTheHat = new List<Cat>() { catGary, catJack, catLuna };

                    if(await db.Cats.CountAsync() < 3)
                    {
                        await db.Cats.AddRangeAsync(catsInTheHat);
                        await db.SaveChangesAsync();
                    }

                    var catsInTheBag = await db.Cats.ToListAsync();

                    foreach(var cat in catsInTheBag)
                    {
                        textView.Text += $"{cat.CatId} - {cat.Name} - {cat.MeowsPerSecond}" + System.Environment.NewLine;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}

