using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace NotificationHubsSamples
{
    /// <summary>
    /// Define the MyBroadcastReceiver class.
    /// </summary>
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        /// <summary>
        /// The senders.
        /// </summary>
        public static string[] SENDEIDS = { NotificationHubsSample.Constants.SenderID };

        /// <summary>
        /// The tag.
        /// </summary>
        public const string TAG = "";
    }

    /// <summary>
    /// Define the GcmService.
    /// </summary>
    [Service]
    public class GcmService : GcmServiceBase
    {
        /// <summary>
        /// Gets the registration identifier.
        /// </summary>
        /// <value>The registration identifier.</value>
        public static string RegistrationID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmService"/> class.
        /// </summary>
        public GcmService()
            : base(NotificationHubsSample.Constants.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GcmService() constructor");
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errorId">The error identifier.</param>
        protected override void OnError(Context context, string errorId)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GcmService() OnError");
        }

        /// <summary>
        /// Called when [registered].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="registrationId">The registration identifier.</param>
        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;
           
            CreateNotification("GcmService-GCM Registered", "The device has been registered in GCM!");
        }

        /// <summary>
        /// Called when [un registered].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="registrationId">The registration identifier.</param>
        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GcmService() OnUnRegistered");
        }
        
        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="intent">The intent.</param>
        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

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
                string messageText = intent.Extras.GetString("msg");
                if (!string.IsNullOrEmpty(messageText))
                {
                    CreateNotification("Message: ", messageText);
                    return;
                }
                messageText = intent.Extras.GetString("message");
                if (!string.IsNullOrEmpty(messageText))
                {
                    CreateNotification("Message: ", messageText);
                    return;
                }
            }

            CreateNotification("Unknown Message:", msg.ToString());
        }

        /// <summary>
        /// Creates the notification.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="desc">The desc.</param>
        private void CreateNotification(string title, string desc)
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