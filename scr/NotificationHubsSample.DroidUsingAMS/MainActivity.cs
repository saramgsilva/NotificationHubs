using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using NotificationHubsSample.WinUsingAMS.Model;

namespace NotificationHubsSample
{
    [Activity(Label = "Register using AMS", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static MobileServiceClient NotificationHubsSampleAMSClient = new MobileServiceClient(Constants.AMSEndpoint, Constants.AMSKey);

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += ButtonClick;

            try
            {
                RegisterWithGCM();
            }
            catch (Exception e)
            {
                // todo handle the exception
                // _logManager.Log(e);
            }

            await NotificationHubsSampleAMSClient.GetPush().RegisterNativeAsync(GcmService.RegistrationID);

            // To register devices using tags
            // var tags = new List<string>();
            // Define the tags before register. Is possible to provide the userId and in backend
            // modify and use the tags based in the usedId (in this case tags should be stored in backend by usedId)
            //await NotificationHubsSampleAMSClient.GetPush().RegisterNativeAsync(GcmService.RegistrationID, tags);
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

        /// <summary>
        /// Buttons the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void ButtonClick(object sender, EventArgs e)
        {
            var carToInsert = new Car
            {
                Name = "Renault 4l",
                IsElectric = false
            };
            var table = NotificationHubsSampleAMSClient.GetTable<Car>();
            await table.InsertAsync(carToInsert);
        }
    }
}

