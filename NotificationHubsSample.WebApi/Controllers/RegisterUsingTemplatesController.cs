using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json.Linq;

namespace NotificationHubsSample.WebApi.Controllers
{
    public class RegisterUsingTemplatesController : ApiController
    {
        // Define the Notification Hubs client.
        private NotificationHubClient _hubClient;

        // Create the client in the constructor.
        public RegisterUsingTemplatesController()
        {
            var cn = "<cn here>";
            _hubClient = NotificationHubClient.CreateClientFromConnectionString(cn, "<hubname here>");
        }
        
        public async Task<RegistrationDescription> Post([FromBody] JObject registrationCall)
        {
            // Get the registration info that we need from the request. 
            var platform = registrationCall["platform"].ToString();
            var installationId = registrationCall["instId"].ToString();
            var channelUri = registrationCall["channelUri"] != null
                                ? registrationCall["channelUri"].ToString()
                                : null;
            var deviceToken = registrationCall["deviceToken"] != null
                                ? registrationCall["deviceToken"].ToString()
                                : null;
            var registrationId = registrationCall["registrationId"] != null
                                    ? registrationCall["registrationId"].ToString() 
                                    : null;

            var userName = HttpContext.Current.User.Identity.Name;

            // Get registrations for the current installation ID.
            var regsForInstId = await _hubClient.GetRegistrationsByTagAsync(installationId, 100);

            bool updated = false;
            bool firstRegistration = true;
            RegistrationDescription registration = null;

            // Check for existing registrations.
            foreach (var registrationDescription in regsForInstId)
            {
                if (firstRegistration)
                {
                    // Update the tags.
                    registrationDescription.Tags = new HashSet<string> { installationId, userName};

                    // We need to handle each platform separately.
                    switch (platform)
                    {
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
                string template;
                switch (platform)
                {
                    case "windows":
                        template = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
                        await _hubClient.CreateWindowsTemplateRegistrationAsync(channelUri, template, new string[] { installationId, userName });
                        break;
                    case "android":
                        template = "{\"data\":{\"message\":\"$(message)\"}}";
                        registration = await _hubClient.CreateGcmTemplateRegistrationAsync(registrationId, template, new[] { installationId, userName });
                        break;
                    case "ios":
                        template = "{\"aps\":{\"alert\":\"$(message)\"}, \"inAppMessage\":\"$(message)\"}";
                        await _hubClient.CreateAppleTemplateRegistrationAsync(deviceToken, template, new string[] { installationId, userName });
                        break;
                }
            }

            // Send out a test notification.
            SendNotification(string.Format("Test notification for {0}", userName), userName);

            return registration;
        }

        private async Task SendNotification(string notificationText, string tag)
        {
            var notification = new Dictionary<string, string> { { "message", notificationText } };
            await _hubClient.SendTemplateNotificationAsync(notification, tag);
        }

        public async Task<bool> ClearRegistrations(string userName)
        {
            // Get registrations for the current installation ID.
            var regsForInstId = await _hubClient.GetRegistrationsByTagAsync(userName, 100);

            // Check for existing registrations.
            foreach (var registrationDescription in regsForInstId)
            {
                // We shouldn't have any extra registrations; delete if we do.
                await _hubClient.DeleteRegistrationAsync(registrationDescription);
            }
            return true;
        }
    }
}
