using System.Diagnostics;
using WindowsAzure.Messaging;
using Foundation;
using NotificationHubs.Xam;
using UIKit;

namespace NotificationHubsSample.Xam.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                UIApplication.SharedApplication.RegisterUserNotificationSettings(
                    UIUserNotificationSettings.GetSettingsForTypes(
                                           UIUserNotificationType.Alert 
                                           | UIUserNotificationType.Badge 
                                           | UIUserNotificationType.Sound, null));
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert 
                                                                         |  UIRemoteNotificationType.Badge 
                                                                         | UIRemoteNotificationType.Sound);
            }
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
     
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // register device in notification hubs
            Hub = new SBNotificationHub(Constants.ConnectionString, Constants.HubName);

            Hub.UnregisterAllAsync(deviceToken, error =>
            {
                if (error != null)
                {
                    Debug.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                // create tags if you want
                NSSet tags = null;
                Hub.RegisterNativeAsync(deviceToken, tags, errorCallback =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                    }
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                var aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                var alert = string.Empty;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying 
                // "  aps:{alert:"alert msg here"}  " this will work fine.
                // But if you're using a complex alert with Localization keys, etc., 
                // your "alert" object from the aps dictionary will be another NSDictionary. 
                // Basically the json gets dumped right into a NSDictionary, 
                // so keep that in mind.
                if (aps != null && aps.ContainsKey(new NSString("alert")))
                {
                    var nsString = aps[new NSString("alert")] as NSString;
                    if (nsString != null)
                    {
                        alert = nsString.ToString();
                    }
                }

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        var avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }
            }
        }
    }
}
