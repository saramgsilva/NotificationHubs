using System;
using System.Diagnostics;
using System.Text;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Gcm;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.Xam.Services;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace NotificationHubsSample.Xam.Droid
{
    /// <summary>
    /// Define the MyBroadcastReceiver.
    /// </summary>
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        /// <summary>
        /// The senderids.
        /// </summary>
        public static string[] Senderids = { Constants.SenderID };

        /// <summary>
        /// The tag.
        /// </summary>
        public const string Tag = "";
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
        public static string RegistrationId { get; private set; }

        /// <summary>
        /// Gets or sets the hub.
        /// </summary>
        /// <value>The hub.</value>
        private NotificationHub Hub { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmService"/> class.
        /// </summary>
        public GcmService()
            : base(Constants.SenderID)
        {
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errorId">The error identifier.</param>
        protected override void OnError(Context context, string errorId)
        {
            Log.Info(MyBroadcastReceiver.Tag, "GcmService() OnError");
        }

        /// <summary>
        /// Called when [registered].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="registrationId">The registration identifier.</param>
        protected override async void OnRegistered(Context context, string registrationId)
        {
            RegistrationId = registrationId;
           
            CreateNotification("GcmService-GCM Registered", "The device has been registered in GCM!");

            Hub = new NotificationHub(Constants.HubName, Constants.ConnectionString, context);
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            try
            {
                var hubRegistration =  Hub.Register(registrationId, settingsService.Tags.ToArray());
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Called when [un registered].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="registrationId">The registration identifier.</param>
        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Info(MyBroadcastReceiver.Tag, "GcmService() OnUnRegistered");
        }


        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="intent">The intent.</param>
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
            var pendingIntent = PendingIntent.GetActivity(this, 0, uiIntent, 0);

            //Create the notification using Notification.Builder
            //Use Android Compatibility Apis

            var notification = new NotificationCompat.Builder(this).SetContentTitle(title)
                .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
                //we use the pending intent, passing our ui intent over which will get called
                //when the notification is tapped.
                .SetContentIntent(pendingIntent)
                .SetContentText(desc)
                //Auto cancel will remove the notification once the user touches it
                 .SetAutoCancel(true).
                Build();

            //Show the notification
            if (notificationManager != null)
            {
                notificationManager.Notify(1, notification);
            }
        }
    }
}