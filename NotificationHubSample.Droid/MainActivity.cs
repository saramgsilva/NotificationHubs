using Android.App;
using Android.Preferences;
using Android.OS;
using Gcm.Client;
using NotificationHubSample.Droid.Views;
using Xamarin.Forms;

namespace NotificationHubSample.Droid
{
    [Activity(Label = "Notification Hub Samples", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Xamarin.Forms.Platform.Android.AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            Forms.Init(this, bundle);

            SettingsHelper.SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this); ;
            
            SetPage(new NavigationPage(new TagsPage(RegisterWithGCM)));
        }

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

