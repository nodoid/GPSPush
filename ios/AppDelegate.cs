using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using CoreLocation;

namespace GPSPush.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private CLLocationManager locationManager;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();

            locationManager = new CLLocationManager
            {
                DesiredAccuracy = CLLocation.AccuracyBest
            };

            locationManager.Failed += (object sender, NSErrorEventArgs e) =>
            {
                var alert = new UIAlertView(){ Title = "Location manager failed", Message = "The location updater has failed" };
                alert.Show();
                locationManager.StopUpdatingLocation();
            };
            
            locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                var newloc = string.Format("{0},{1}", e.Locations[0].Coordinate.Longitude, e.Locations[0].Coordinate.Latitude);
                App.Self.ChangedClass.BroadcastIt("location", newloc);
            };

            locationManager.StartUpdatingLocation();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

