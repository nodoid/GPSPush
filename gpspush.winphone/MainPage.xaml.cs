using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GPSPush.WinPhone.Resources;
using Xamarin.Forms.Platform.WinPhone;
using Xamarin.Forms;

// used for notifications
using Microsoft.Phone.Notification;
using System.IO;

// used for geolocation
using Windows.Devices.Geolocation;

namespace GPSPush.WinPhone
{
    public partial class MainPage : FormsApplicationPage
    {
        // Constructor
        public MainPage()
        {
            // the push channel that is created or used
            // rawdata is the name of the push channel
            var pushNotification = HttpNotificationChannel.Find("rawdata");

            InitializeComponent();

            // channel not found, so create one
            if (pushNotification == null)
            {
                pushNotification = new HttpNotificationChannel("rawdata");

                // register the events

                pushNotification.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(ChannelUriUpdated);
                pushNotification.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(ErrorOccurred);
                pushNotification.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(NewNotification);
                pushNotification.Open();
            }
            else
            {
                // channel exists, so just register the events

                pushNotification.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(ChannelUriUpdated);
                pushNotification.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(ErrorOccurred);
                pushNotification.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(NewNotification);
            }

            // get the location data
            GetLocation();

            Forms.Init();
            LoadApplication(new GPSPush.App());
        }

        async void GetLocation()
        {
            // set up the geoLocation constructor
            var geoLocation = new Geolocator
            {
                DesiredAccuracyInMeters = 50
            };
            var geoData = new double[] {0,0};
            try
            {
                // get the position
                var position = await geoLocation.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
                geoData[0] = position.Coordinate.Point.Position.Latitude;
                geoData[1] = position.Coordinate.Point.Position.Longitude;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown - {0}:{1}", ex.Message, ex.InnerException);
            }
            // broadcast for the event handler to catch
            GPSPush.App.Self.ChangedClass.BroadcastIt(geoData[0].ToString(), geoData[1].ToString());
        }

        void ChannelUriUpdated(object s, NotificationChannelUriEventArgs e)
        {
            Console.WriteLine("New uri {0}", e.ChannelUri.ToString());
        }

        void ErrorOccurred(object s, NotificationChannelErrorEventArgs e)
        {
            Console.WriteLine("Push error - {0}", e.Message);
        }

        void NewNotification(object s, HttpNotificationEventArgs e)
        {
            var message = string.Empty;
            using (var reader = new StreamReader(e.Notification.Body))
                message = reader.ReadToEnd();

            var mSplit = message.Split(',');

            GPSPush.App.Self.ChangedClass.BroadcastIt(mSplit[0], mSplit[1]);
        }
    }
}