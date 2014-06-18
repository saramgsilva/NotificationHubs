using System;
using System.Diagnostics;
using System.Text;
using Android.App;
using Android.Content;
using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace NotificationHubSample.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = { Constants.SenderID };

        public const string TAG = "";
    }

    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }

        public GcmService()
            : base(Constants.SenderID)
        {
        }

        protected override void OnError(Context context, string errorId)
        {
        }

        protected override async void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;
           
            createNotification("GcmService-GCM Registered", "The device has been registered in GCM!");

            Hub = new NotificationHub(Constants.NotificationHubPath, Constants.ConnectionString);
           
            if (!string.IsNullOrEmpty(SettingsHelper.RegistrationId))
            {
                try
                {
                    await Hub.UnregisterAllAsync(registrationId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                }
            }

            try
            {
                var hubRegistration = await Hub.RegisterNativeAsync(registrationId, SettingsHelper.Tags);
                
                SettingsHelper.RegistrationId = registrationId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
        }


        protected override void OnMessage(Context context, Intent intent)
        {
            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    msg.AppendLine(key + "=" + intent.Extras.Get(key));
                }
            }
            if (intent != null && intent.Extras != null)
            {
                var messageText = intent.Extras.GetString("message");
                if (!string.IsNullOrEmpty(messageText))
                {
                    createNotification("Message: ", messageText);
                    return;
                }
            }

            createNotification("Unknown Message:", msg.ToString());
        }

        private void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
        }
    }
}