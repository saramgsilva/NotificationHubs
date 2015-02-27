﻿using System;
using Android.App;
using Android.OS;
using Android.Preferences;
using Gcm.Client;
using NotificationHubsSample.Views;
using Xamarin.Forms;

namespace NotificationHubsSample
{
    [Activity(Label = "Register using WebApi (v2)", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Xamarin.Forms.Platform.Android.AndroidActivity
    {
        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
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

