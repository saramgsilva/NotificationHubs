using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Newtonsoft.Json;

namespace NotificationHubSamples.Win8UsingWebApi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _registerUri = "https://<your service here>.azurewebsites.net/api/register";
        private string _registerUsingTemplatesUri = "https://<your service here>.azurewebsites.net/api/RegisterUsingTemplates";
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Thickness MarginForGrid 
        {
            get
            {
#if WINDOWS_APP
                return new Thickness(120, 58, 120, 80);
#else
                return new Thickness(20, 58, 20, 80);
#endif
            }
        }

        // This method gets both an installation ID and channel for push notifications and sends it, 
        // along with the device type, to the authenticated Web API method that creates a registration 
        // in Notification Hubs.
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            // Get the info that we need to request registration.
            var installationId = InstId.Text = SettingsHelper.GetInstallationId();
           
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += channel_PushNotificationReceived;
            
            var user = User.Text;
            var pwd = Password.Password;

            // Define a registration to pass in the body of the POST request.
            var registration = new Dictionary<string, string>()
            {
                { "platform", "windows" },
                { "instId", installationId },
                { "channelUri", channel.Uri }
            };

            // Create a client to send the HTTP registration request.
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_registerUri));

            // Add the Authorization header with the basic login credentials.
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pwd));
            request.Headers.Add("Authorization", auth);
            request.Content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

            string message;

            try
            {
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
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            string result = args.NotificationType.ToString();

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
