using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using Gcm.Client;
using NotificationHubs.Xam.Droid.Services;
using Constants = NotificationHubsSample.Constants;

namespace NotificationHubs.Xam.Droid
{
    [Activity(Label = "NotificationHubs.Xam", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            SimpleIoc.Default.Register<IAMSClient, AMSClient>();
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

        /// <summary>
        /// Registers the with GCM.
        /// </summary>
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

