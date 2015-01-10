using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;

namespace NotificationHubsSample.AMS
{
    /// <summary>
    /// The push helper.
    /// </summary>
    public static class PushHelper
    {
        /// <summary>
        /// Gets the windows push message for toast text01.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The IPushMessage.</returns>
        public static IPushMessage GetWindowsPushMessageForToastText01(string message)
        {
            var payload = new XElement("toast",
                         new XElement("visual",
                            new XElement("binding",
                                new XAttribute("template", "ToastText01"),
                                new XElement("text",
                                    new XAttribute("id", "1"), message)))).ToString(SaveOptions.DisableFormatting);

            return new WindowsPushMessage
            {
                XmlPayload = payload
            };
        }
        /// <summary>
        /// Gets the google push message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>IPushMessage.</returns>
        public static IPushMessage GetGooglePushMessage(string message)
        {
            var data = new Dictionary<string, string>
            {
                    { "message", message }
            };

            return new GooglePushMessage(data, TimeSpan.FromHours(1));
        }

        /// <summary>
        /// Gets the apple push message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>IPushMessage.</returns>
        public static IPushMessage GetApplePushMessage(string message)
        {
            return new ApplePushMessage(message, TimeSpan.FromHours(1));
        }
    }
}