using System;

using Xamarin.Forms;
using System.Linq;

namespace GPSPush
{
    public class App : Application
    {
        public static App Self { get; private set; }

        public ChangedEvent ChangedClass { get; set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public App()
        {
            App.Self = this;
            ChangedClass = new ChangedEvent();

            // we need give the co-ordinates an initial value
            Longitude = 53.431;
            Latitude = -2.956;

            this.ChangedClass.Change += (object s, ChangedEventArgs ea) =>
            {
                if (ea.ModuleName == "location")
                {
                    var locdata = ea.Id.Split(',').ToArray();
                    Longitude = double.Parse(locdata[1]);
                    Latitude = double.Parse(locdata[0]);

                    ChangedClass.BroadcastIt("updated-location", ".");
                }
            };

            MainPage = new MapPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

