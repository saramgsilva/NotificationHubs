using Android.App;
using Android.Content.PM;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using Gcm.Client;
using NotificationHubsSample.Xam.Droid.Services;
using NotificationHubsSample.Xam.Services;

namespace NotificationHubsSample.Xam.Droid
{
    [Activity(Label = "Notification Hubs Sample ", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RegisterInGcm();

            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        private void RegisterInGcm()
        {
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);
            GcmClient.Register(this, Constants.SenderID);
        }
    }
}

