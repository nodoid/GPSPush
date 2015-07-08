using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace GPSPush.Droid
{
    [Activity(Label = "GPSPush.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, ILocationListener
    {
        LocationManager locationManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);
            locationManager = GetSystemService(Context.LocationService) as LocationManager;

            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            var provider = LocationManager.GpsProvider;
            if (locationManager.IsProviderEnabled(provider))
                locationManager.RequestLocationUpdates(provider, 2000, 1, this);
        }

        public void OnLocationChanged(Location location)
        {
            var loc = string.Format("{0},{1}", location.Longitude, location.Latitude);
            App.Self.ChangedClass.BroadcastIt("location", loc);
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}

