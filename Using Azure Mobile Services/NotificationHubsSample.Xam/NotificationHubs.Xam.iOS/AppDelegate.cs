using Foundation;
using GalaSoft.MvvmLight.Ioc;
using NotificationHubs.Xam;
using NotificationHubs.Xam.Droid.Services;
using System.Diagnostics;
using Microsoft.Practices.ServiceLocation;
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
            // registers for push for iOS8
            var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert
                | UIUserNotificationType.Badge
                | UIUserNotificationType.Sound,
                new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            SimpleIoc.Default.Register<IAMSClient, AMSClient>();
            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Indicates that a call to <see cref="M:UIKit.UIApplication.RegisterForRemoteNotifications" /> succeeded.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <param name="deviceToken">To be added.</param>
        /// <remarks>To be added.</remarks>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Modify device token
            DeviceToken = deviceToken.Description;
            DeviceToken = DeviceToken.Trim('<', '>').Replace(" ", "");
            
            var amsClient = ServiceLocator.Current.GetInstance<IAMSClient>();
            amsClient.RegisterNativateAsync(DeviceToken);
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
            Debug.WriteLine(userInfo.ToString());
            NSObject alertMessage;

            bool success = userInfo.TryGetValue(new NSString("alert"), out alertMessage);

            if (success)
            {
                var alert = new UIAlertView("Got push notification", alertMessage.ToString(), null, "OK", null);
                alert.Show();
            }
        }
    }
}
