using System;
using System.Collections.Generic;
using System.IO;
using EntityFrameworkWithXamarin.Core;
using Microsoft.EntityFrameworkCore;
using UIKit;

namespace EntityFrameworkWithXamarin.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override async void ViewDidLoad()
        {
            System.Diagnostics.Debug.WriteLine("ViewController.ViewDidLoad()");
            base.ViewDidLoad();

            // NOTE: See LinkDescription.xml, which describes how to get the Xamarin linker to preserve key types and
            // methods that Entity Framework Core uses only via reflection. Without such instructions to the linker,
            // EF Core code such as below would generate runtime errors when the linker is used; e.g. in builds that
            // are deployed to device or for publishing to the App Store. Unfortunately, such issues aren't immediately
            // evident when running a sample like this in the simulator, as the default Xamarin iOS project config has
            // linking disabled for the iPhoneSimulator target.

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

                    if (await db.Cats.CountAsync() < 3)
                    {
                        await db.Cats.AddRangeAsync(catsInTheHat);
                        await db.SaveChangesAsync();
                    }

                    var catsInTheBag = await db.Cats.ToListAsync();

                    foreach (var cat in catsInTheBag)
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

        partial void Button_TouchUpInside(UIButton sender)
        {
            // The button logic isn't related to EF Core but is here to mimic the Droid sample project.
            button.SetTitle(string.Format("{0} clicks!", _count++), UIControlState.Normal);
        }

        private static int _count = 1;
    }
}