using System;
using Android.App;
using Android.Preferences;
using Android.OS;
using Gcm.Client;
using NotificationHubsSamples.DroidUsingWebApi.Views;
using Xamarin.Forms;

namespace NotificationHubsSamples.DroidUsingWebApi
{
    [Activity(Label = "Register using WebApi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Xamarin.Forms.Platform.Android.AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SettingsHelper.SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this); ;

            Forms.Init(this, bundle);
            SetPage(new NavigationPage(new MainPage()));

            try
            {
                RegisterWithGCM();
            }
            catch (Exception e)
            {
                // todo handle the exception
                // _logManager.Log(e);
            }
        }


        public void RegisterWithGCM()
        {
            // Check to ensure everything's setup right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            System.Diagnostics.Debug.WriteLine("Registering the device: " + Constants.SenderID);
            GcmClient.Register(this, Constants.SenderID);
        }
    }
}

