using Android.App;
using Android.Content;

namespace GPSPush.Droid
{
    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new string[]{ "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[]{ "@PACKAGE_NAME" })]
    [IntentFilter(new string[]{ "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[]{ "@PACKAGE_NAME@" })]
    [IntentFilter(new string[]{ "com.google.android.gcm.intent.RETRY" }, Categories = new string[]{ "@PACKAGE_NAME@" })]
    [IntentFilter(new string[]{ Android.Content.Intent.ActionBootCompleted })]
    public class GPSDroidReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            GPSDroidService.RunIntentInService(context, intent);
            SetResult(Result.Ok, null, null);
        }
    }
}

