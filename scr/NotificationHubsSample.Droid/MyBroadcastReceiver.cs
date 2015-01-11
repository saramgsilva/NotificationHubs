using System;
using System.Diagnostics;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace NotificationHubsSample
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
        public static string[] SENDERIDS = { Constants.SenderID };

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
            Log.Info(MyBroadcastReceiver.TAG, "GcmService() OnError");
        }

        /// <summary>
        /// Called when [registered].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="registrationId">The registration identifier.</param>
        protected override async void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;
           
            CreateNotification("GcmService-GCM Registered", "The device has been registered in GCM!");

            Hub = new NotificationHub(Constants.HubName, Constants.ConnectionString);
           
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
                var hubRegistration = await Hub.RegisterNativeAsync(registrationId);

                #region to use tags
                // var hubRegistration = await Hub.RegisterNativeAsync(registrationId, SettingsHelper.Tags);
                #endregion

                SettingsHelper.RegistrationId = registrationId;
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
            Log.Info(MyBroadcastReceiver.TAG, "GcmService() OnUnRegistered");
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