using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Newtonsoft.Json;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the MainPage.
    /// </summary>
    public sealed partial class MainPage
    {
        private const string RegisterUrl = "https://notificationhubssamplewebapiv2.azurewebsites.net/api/NotificationHub/Register";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// This method gets both an installation ID and channel for push notifications and sends it, 
        /// along with the device type, to the authenticated Web API method that creates a registration 
        /// in Notification Hubs.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            // Get the info that we need to request registration.
            var registrationId = InstId.Text = SettingsHelper.GetRegistrationId();

            string message;

            try
            {
                var user = User.Text;
                var pwd = Password.Password;

                // Define a registration to pass in the body of the POST request.
                var deviceInfo = new DeviceInfo
                {
                    Platform = "mpns",
                    Handle = App.NotificationService.ChannelUri,
                    RegistrationId = registrationId,
                    Tags = new List<string>()
                };


                // Create a client to send the HTTP registration request.
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(RegisterUrl));

                // Add the Authorization header with the basic login credentials.
                var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pwd));
                request.Headers.Add("Authorization", auth);
                request.Content = new StringContent(JsonConvert.SerializeObject(deviceInfo), Encoding.UTF8, "application/json");


                // Send the registration request.
                var response = await client.SendAsync(request);

                // Get and display the response
                message = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            MessageBox.Show(message);
        }
 }
}