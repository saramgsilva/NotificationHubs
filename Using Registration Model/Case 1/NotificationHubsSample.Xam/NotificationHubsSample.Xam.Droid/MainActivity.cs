using Android.App;
using Android.Content.PM;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
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
            var settingsService = new SettingsService();
            settingsService.Init(this);

            var notificationHubsService = new NotificationHubsService();
            notificationHubsService.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(settingsService, notificationHubsService));
            notificationHubsService.RegisterOrUpdate();
        }
    }
}

