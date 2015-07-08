using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSPush
{
    public class MapPage : ContentPage
    {
        double lon, lat;
        Map myMap;

        public MapPage()
        {
            lon = App.Self.Longitude;
            lat = App.Self.Latitude;

            myMap = new Map(MapSpan.FromCenterAndRadius(new Position(App.Self.Longitude, App.Self.Latitude), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            App.Self.ChangedClass.Change += HandleMapChanged;

            Content = new StackLayout
            { 
                Spacing = 0,
                Children =
                {
                    myMap
                }
            };
        }

        void HandleMapChanged(object s, ChangedEventArgs ea)
        {
            if (ea.ModuleName == "updated-location")
            {
                if (lon != App.Self.Longitude || lat != App.Self.Latitude)
                {
                    myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.Self.Longitude, App.Self.Latitude), Distance.FromMiles(0.3)));
                    lon = App.Self.Longitude;
                    lat = App.Self.Latitude;
                }
            } 
        }
    }
}


