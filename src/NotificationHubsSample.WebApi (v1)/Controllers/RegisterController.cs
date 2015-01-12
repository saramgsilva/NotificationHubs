using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json.Linq;

namespace NotificationHubsSample.WebApi.Controllers
{
    /// <summary>
    /// Define the RegisterController.
    /// </summary>
    public class RegisterController : ApiController
    {
        // Define the Notification Hubs client.
        private readonly NotificationHubClient _hubClient;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterController"/> class.
        /// </summary>
        public RegisterController()
        {
            // Create the client in the constructor.
            _hubClient = NotificationHubClient.CreateClientFromConnectionString(Constants.BackEndConnectionString, Constants.HubName);
        }

        /// <summary>
        /// Posts the specified registration call.
        /// </summary>
        /// <param name="registrationCall">The registration call.</param>
        /// <returns>Task&lt;RegistrationDescription&gt;.</returns>
        public async Task<RegistrationDescription> PostAsync([FromBody]JObject registrationCall)
        {
            // Get the registration info that we need from the request. 
            var platform = registrationCall["platform"].ToString();
            var installationId = registrationCall["instId"].ToString();
            var channelUri = registrationCall["channelUri"] != null ? registrationCall["channelUri"].ToString() : null;
            var deviceToken = registrationCall["deviceToken"] != null ? registrationCall["deviceToken"].ToString() : null;
            var registrationId = registrationCall["registrationId"] != null ? registrationCall["registrationId"].ToString() : null;

            var userName = HttpContext.Current.User.Identity.Name;

            // Get registrations for the current installation ID.
            var regsForInstId = await _hubClient.GetRegistrationsByTagAsync(installationId, 100);

            var updated = false;
            var firstRegistration = true;
            RegistrationDescription registration = null;

            // Check for existing registrations.
            foreach (var registrationDescription in regsForInstId)
            {
                if (firstRegistration)
                {
                    // Update the tags.
                    // Note: the tags can be from the device or from the backEnd storage, 
                    // it depends from the application requirements and in this case are used the installationid and the userName as example
                    registrationDescription.Tags = new HashSet<string> { installationId, userName };

                    // We need to handle each platform separately.
                    switch (platform)
                    {
                        case "mpns":
                            var mpns = registrationDescription as MpnsRegistrationDescription;
                            if (mpns != null)
                            {
                                if (channelUri != null)
                                {
                                    mpns.ChannelUri = new Uri(channelUri);
                                }
                                registration = await _hubClient.UpdateRegistrationAsync(mpns);
                            }
                            break;
                        case "windows":
                            var winReg = registrationDescription as WindowsRegistrationDescription;
                            if (winReg != null)
                            {
                                if (channelUri != null)
                                {
                                    winReg.ChannelUri = new Uri(channelUri);
                                }
                                registration = await _hubClient.UpdateRegistrationAsync(winReg);
                            }
                            break;
                        case "android":
                            var gcmReg = registrationDescription as GcmRegistrationDescription;
                            if (gcmReg != null)
                            {
                                gcmReg.GcmRegistrationId = registrationId;
                                registration = await _hubClient.UpdateRegistrationAsync(gcmReg);
                            }
                            break;
                        case "ios":
                            var iosReg = registrationDescription as AppleRegistrationDescription;
                            if (iosReg != null)
                            {
                                iosReg.DeviceToken = deviceToken;
                                registration = await _hubClient.UpdateRegistrationAsync(iosReg);
                            }
                            break;
                    }
                    updated = true;
                    firstRegistration = false;
                }
                else
                {
                    // We shouldn't have any extra registrations; delete if we do.
                    await _hubClient.DeleteRegistrationAsync(registrationDescription);
                }
            }

            // Create a new registration.
            if (!updated)
            {
                // Note: the tags can be from the device or from the backEnd storage, 
                // it depends from the application requirements and in this case are used the installationid and the userName as example
                
                switch (platform)
                {
                    case "mpns":
                        registration = await _hubClient.CreateMpnsNativeRegistrationAsync(channelUri, new[] { installationId, userName });
                        break;
                    case "windows":
                        registration = await _hubClient.CreateWindowsNativeRegistrationAsync(channelUri, new[] { installationId, userName });
                        break;
                    case "android":
                        registration = await _hubClient.CreateGcmNativeRegistrationAsync(registrationId, new[] { installationId, userName });
                        break;
                    case "ios":
                        registration = await _hubClient.CreateAppleNativeRegistrationAsync(deviceToken, new[] { installationId, userName });
                        break;
                }
            }

            // Send out a test notification.
            await SendNotificationAsync(string.Format("Test notification for {0}", userName), userName);

            return registration;
        }

        /// <summary>
        /// Basic implementation that sends a notification to Windows Store, Android and iOS app clients.
        /// </summary>
        /// <param name="notificationText">The notification text.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>Task.</returns>
        private async Task SendNotificationAsync(string notificationText, string tag)
        {
            try
            {
                XNamespace wp = "WPNotification";
                XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null),
                    new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"),
                        new XElement(wp + "Toast",
                            new XElement(wp + "Text1",
                                 "Notification Hubs Sample"),
                            new XElement(wp + "Text2", notificationText))));

                var toastMpns = string.Concat(doc.Declaration, doc.ToString(SaveOptions.DisableFormatting));

                // Create notifications for both Windows Store and iOS platforms.
                var toast = new XElement("toast",
                              new XElement("visual",
                                  new XElement("binding",
                                      new XAttribute("template", "ToastText01"),
                                      new XElement("text",
                                          new XAttribute("id", "1"),
                                          notificationText)))).ToString(SaveOptions.DisableFormatting);
                var alert =new JObject(
                                new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
                                new JProperty("inAppMessage", notificationText))
                                .ToString(Newtonsoft.Json.Formatting.None);

                var payload = new JObject(
                                    new JProperty("data", new JObject(new JProperty("message", notificationText))))
                                    .ToString(Newtonsoft.Json.Formatting.None);

                // Send a notification to the logged-in user on both platforms.
       
                var googleResult = await _hubClient.SendGcmNativeNotificationAsync(payload, tag);

                var windowsResult = await _hubClient.SendWindowsNativeNotificationAsync(toast, tag);

                var mpnsResult = await _hubClient.SendMpnsNativeNotificationAsync(toastMpns, tag);

                //var appleResult = await _hubClient.SendAppleNativeNotificationAsync(alert, tag);
            }
            catch (ArgumentException ex)
            {
                // This is expected when an APNS registration doesn't exist.
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Clears the registrations.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Task.</returns>
        public async Task ClearRegistrationsAsync(string userName)
        {
            // Get registrations for the current installation ID.
            var regsForInstId = await _hubClient.GetRegistrationsByTagAsync(userName, 100);

            // Check for existing registrations.
            foreach (var registrationDescription in regsForInstId)
            {
                // We shouldn't have any extra registrations; delete if we do.
                await _hubClient.DeleteRegistrationAsync(registrationDescription);
            }
        }
    }
}
