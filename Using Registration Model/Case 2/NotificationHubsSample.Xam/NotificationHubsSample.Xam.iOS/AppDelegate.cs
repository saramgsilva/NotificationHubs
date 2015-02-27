using Foundation;
using GalaSoft.MvvmLight.Ioc;
using NotificationHubsSample.Xam.iOS.Services;
using NotificationHubsSample.Xam.Services;
using UIKit;

namespace NotificationHubsSample.Xam.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public string DeviceToken { get; set; }

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
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
     
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            DeviceToken = deviceToken.Description;
            DeviceToken = DeviceToken.Trim('<', '>').Replace(" ", "");
            var myservice = SimpleIoc.Default.GetInstance<IMyWebService>();
            myservice.PnsHandler = DeviceToken;
            myservice.Platform = "apns";
        }

        /// <summary>
        /// Indicates that the application received a remote notification.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="userInfo">A dictionary whose "aps" key contains information related to the notification</param>
        /// <remarks><para>The <paramref name="userInfo" /> dictionary will have a key <c>aps</c> that will return another <see cref="T:Foundation.NSDictionary" />. That dictionary may include the following keys:</para>
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Type</description>
        ///     <description>Description</description>
        ///   </listheader>
        ///   <item>
        ///     <term>alert</term>
        ///     <description>String or <see cref="T:Foundation.NSDictionary" /></description>
        ///     <description>If the value for the <c>alert</c> key is a <see langword="string" />, that string will be the text of an alert with two buttons: "Close" and "View". If the application user choose "View", the application will launch. If the value is a <see cref="T:Foundation.NSDictionary" />, it will contain a series of keys relating to localization.</description>
        ///   </item>
        ///   <item>
        ///     <term>badge</term>
        ///     <description>Integer</description>
        ///     <description>The number to display on the badge of the app icon. If 0, the badge will be removed. If <see langword="null" />, the badge should not change.</description>
        ///   </item>
        ///   <item>
        ///     <term>sound</term>
        ///     <description>String</description>
        ///     <description>The name of a sound file in the app bundle. If the file doesn't exist or the value is "default", the default alert sound will be played.</description>
        ///   </item>
        ///   <item>
        ///     <term>content-available</term>
        ///     <description>Integer</description>
        ///     <description>A value of 1 indicates that new content is available. This is intended for Newsstand apps and background content downloads.</description>
        ///   </item>
        /// </list></remarks>
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
