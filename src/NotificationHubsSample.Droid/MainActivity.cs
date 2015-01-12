using Android.App;
using Android.OS;
using Android.Preferences;
using Gcm.Client;
using NotificationHubsSample.Views;
using Xamarin.Forms;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the MainActivity.
    /// </summary>
    [Activity(Label = "Notification Hub Samples", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Xamarin.Forms.Platform.Android.AndroidActivity
    {
        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            Forms.Init(this, bundle);

            SettingsHelper.SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this); ;

            SetPage(new NavigationPage(new RegisterPage(RegisterWithGCM)));

            #region To use tags
            // SetPage(new NavigationPage(new TagsPage(RegisterWithGCM)));
            #endregion
        }

        /// <summary>
        /// Registers the with GCM.
        /// </summary>
        public void RegisterWithGCM()
        {
            // Check to ensure everything's setup right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            GcmClient.Register(this, Constants.SenderID);
        }
    }
}

