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
    public sealed partial class MainPage
    {
        private const string RegisterUrl = "https://NotificationHubsSampleWebApi.azurewebsites.net/api/register";

        // In this case the templates are defined when is registered the decive
        private const string RegisterUsingTemplatesUrl = "https://NotificationHubsSampleWebApi.azurewebsites.net/api/RegisterUsingTemplates";

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
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            // Get the info that we need to request registration.
            var installationId = InstId.Text = SettingsHelper.GetInstallationId();

            string message;

            try
            {
                var user = User.Text;
                var pwd = Password.Password;

                // Define a registration to pass in the body of the POST request.
                var registration = new Dictionary<string, string>()
                {
                    { "platform", "mpns" },
                    { "instId", installationId },
                    { "channelUri", App.NotificationService.ChannelUri }
                };

                // Create a client to send the HTTP registration request.
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(RegisterUrl));

                // Add the Authorization header with the basic login credentials.
                var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pwd));
                request.Headers.Add("Authorization", auth);
                request.Content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");


                // Send the registration request.
                var response = await client.SendAsync(request);

                // Get and display the response, either the registration ID
                // or an error message.
                message = await response.Content.ReadAsStringAsync();
                message = string.Format("Registration ID: {0}", message);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display a message dialog.
            MessageBox.Show(message);
        }

        /// <summary>
        /// Channels the push notification received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PushNotificationReceivedEventArgs"/> instance containing the event data.</param>
        private void ChannelPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            var result = args.NotificationType.ToString();

            switch (args.NotificationType)
            {
                case PushNotificationType.Badge:
                    if (args.BadgeNotification != null)
                    {
                        result += ": " + args.BadgeNotification.Content.GetXml();
                        var updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
                        updater.Update(new BadgeNotification(args.BadgeNotification.Content));
                    }
                    break;
                case PushNotificationType.Raw:
                    if (args.RawNotification != null)
                    {
                        result += ": " + args.RawNotification.Content;
                        // todo something here...
                    }
                    break;
                case PushNotificationType.Tile:
                    if (args.TileNotification != null)
                    {
                        result += ": " + args.TileNotification.Content.GetXml();
                        var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                        updater.Update(new TileNotification(args.TileNotification.Content));
                    }
                    break;
                case PushNotificationType.Toast:
                    if (args.ToastNotification != null)
                    {
                        result += ": " + args.ToastNotification.Content.GetXml();
                        // todo something here...
                    }
                    break;
                default:
                    break;
            }
        }
    }
}