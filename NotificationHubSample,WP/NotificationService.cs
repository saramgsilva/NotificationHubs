using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace NotificationHubSample_WP
{
    public class NotificationService
    {
        private const string HubName = "<hubname here>";
        private const string ConnectionString = "<cs here>";
        
        /// <summary>
        /// The create or update notifications async.
        /// </summary>
        public static async void CreateOrUpdateNotificationsAsync()
        {
            try
            {
                // get the channel for the app
                var pushNotificationChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                pushNotificationChannel.PushNotificationReceived += pushNotificationChannel_PushNotificationReceived;
                
                var hub = new NotificationHub(HubName, ConnectionString);

                // unregister
                if (!string.IsNullOrEmpty(SettingsHelper.ChannelUri))
                {
                    await hub.UnregisterAllAsync(SettingsHelper.ChannelUri);
                    SettingsHelper.RemoveAll();
                }

                // register the channel in NH
              
                var result = await hub.RegisterNativeAsync(pushNotificationChannel.Uri, SettingsHelper.Tags);
                
                if (result.RegistrationId != null)
                {
                    await ShowRegistrationId(result);

                    // save data
                    SettingsHelper.ChannelUri = result.ChannelUri;
                    SettingsHelper.RegistrationId = result.RegistrationId;
                }
            }
            catch (Exception ex)
            {
                //todo handle the exception
                // _logManager.Log(ex);
            }
        }

        private static async Task ShowRegistrationId(Registration result)
        {
            var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        static void pushNotificationChannel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
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
