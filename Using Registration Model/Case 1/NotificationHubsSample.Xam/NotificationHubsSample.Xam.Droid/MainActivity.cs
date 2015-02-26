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

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new SettingsService(), new NotificationHubsService()));

            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.Init(this);
            var notificationHubsService = ServiceLocator.Current.GetInstance<INotificationHubsService>();
            notificationHubsService.Init(this);
            notificationHubsService.RegisterOrUpdate();
        }
    }
}

