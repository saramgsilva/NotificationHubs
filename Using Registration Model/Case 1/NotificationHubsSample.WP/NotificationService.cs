using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace NotificationHubsSample
{
    public class NotificationService
    {
        /// <summary>
        /// The create or update notifications async.
        /// </summary>
        public static async void CreateOrUpdateNotificationsAsync()
        {
            try
            {
                // get the channel for the app
                var pushNotificationChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                pushNotificationChannel.PushNotificationReceived += PushNotificationChannelPushNotificationReceived;

                var hub = new NotificationHub(Constants.HubName, Constants.ConnectionString);
                
                // register the channel in NH
                
                var result = await hub.RegisterNativeAsync(pushNotificationChannel.Uri, SettingsHelper.Tags);
                if (result.RegistrationId != null)
                {
                    await ShowRegistrationIdAsync(result);
                }
            }
            catch (Exception ex)
            {
                //todo handle the exception
                // _logManager.Log(ex);
            }
        }

        /// <summary>
        /// Shows the registration identifier.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        private static async Task ShowRegistrationIdAsync(Registration result)
        {
            var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Pushes the notification channel push notification received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PushNotificationReceivedEventArgs"/> instance containing the event data.</param>
        private static void PushNotificationChannelPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
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
