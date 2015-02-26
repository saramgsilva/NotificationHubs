using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Microsoft.Phone.Notification;
using Microsoft.WindowsAzure.Messaging;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the NotificationService.
    /// </summary>
    public class NotificationService
    {
        /// <summary>
        /// Creates the or update notifications asynchronous.
        /// </summary>
        public void CreateOrUpdateNotificationsAsync()
        {
           var channel = HttpNotificationChannel.Find("MyPushChannel");

            if (channel == null)
            {
                channel = new HttpNotificationChannel("MyPushChannel");
                channel.Open();
                channel.BindToShellToast();

                channel.ErrorOccurred += Channel_ErrorOccurred;
                channel.HttpNotificationReceived += Channel_HttpNotificationReceived;
                channel.ShellToastNotificationReceived += Channel_ShellToastNotificationReceived;
                channel.ChannelUriUpdated += Channel_ChannelUriUpdated;
            }
            else
            {
                channel.ErrorOccurred += Channel_ErrorOccurred;
                channel.HttpNotificationReceived += Channel_HttpNotificationReceived;
                channel.ShellToastNotificationReceived += Channel_ShellToastNotificationReceived;
                channel.ChannelUriUpdated += Channel_ChannelUriUpdated;

                Debug.WriteLine(channel.ChannelUri.ToString());
            }
        }

        /// <summary>
        /// Handles the ChannelUriUpdated event of the Channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotificationChannelUriEventArgs"/> instance containing the event data.</param>
        private async void Channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
                try
                {
                    var hub = new NotificationHub(Constants.HubName, Constants.ConnectionString);

                    Debug.WriteLine(e.ChannelUri.ToString());

                    // register the channel in NH without tags
                    //var result = await hub.RegisterNativeAsync(e.ChannelUri.ToString());
                    var result = await hub.RegisterNativeAsync(e.ChannelUri.ToString(), SettingsHelper.Tags);


                    if (result.RegistrationId != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show(result.RegistrationId);
                        });
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
        }

        /// <summary>
        /// Handles the ShellToastNotificationReceived event of the Channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotificationEventArgs"/> instance containing the event data.</param>
        private void Channel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();

            message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                }
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
               MessageBox.Show(message.ToString());

            });
        }

        /// <summary>
        /// Handles the ErrorOccurred event of the Channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotificationChannelErrorEventArgs"/> instance containing the event data.</param>
        private void Channel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }

        /// <summary>
        /// Handles the HttpNotificationReceived event of the Channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HttpNotificationEventArgs"/> instance containing the event data.</param>
        private void Channel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            Debug.WriteLine(e.Notification);
        }
    }
}
