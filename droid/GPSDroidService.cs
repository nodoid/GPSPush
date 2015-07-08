using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace GPSPush.Droid
{
    [Service]			
    public class GPSDroidService : IntentService
    {
        static PowerManager.WakeLock wakeLock;
        static object LOCK = new object();

        static void RunIntentInService(Context context, Intent intent)
        {
            lock (LOCK)
            {
                if (wakeLock == null)
                {
                    var pm = PowerManager.FromContext(context);
                    wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "Woken lock");
                }
            }

            wakeLock.Acquire();
            intent.SetClass(context, typeof(GPSDroidService));
            context.StartService(intent);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                var context = ApplicationContext;
                var action = intent.Action;

                switch (action)
                {
                    case "com.google.android.c2dm.intent.REGISTRATION":
                        var senders = "MySenderID";
                        var regIntent = new Intent("com.google.android.c2dm.intent.REGISTER");
                        regIntent.SetPackage("com.google.android.gsf");
                        regIntent.PutExtra("app", PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
                        regIntent.PutExtra("sender", senders);
                        context.StartService(regIntent);
                        break;
                    case "com.google.android.c2dm.UNREGISTER":
                        var unregIntent = new Intent("com.google.android.c2dm.intent.UNREGISTER");
                        unregIntent.PutExtra("app", PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
                        context.StartService(regIntent);
                        break;
                    case "com.google.android.c2dm.intent.RECEIVE":
                        var eventid = intent.GetStringExtra("eventid");
                        var eventname = intent.GetStringExtra("eventname");
                        if (!string.IsNullOrEmpty(eventname) &&)
                            App.Self.ChangedClass.BroadcastIt(eventname);
                        break;
                }
            }
            finally
            {
                lock (LOCK)
                {
                    if (wakeLock != null)
                        wakeLock.Release();
                }
            }
        }
    }
}

