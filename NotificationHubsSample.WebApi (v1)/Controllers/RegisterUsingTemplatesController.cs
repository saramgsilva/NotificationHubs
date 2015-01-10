using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json.Linq;

namespace NotificationHubsSample.WebApi.Controllers
{
    /// <summary>
    /// Define the RegisterUsingTemplatesController.
    /// </summary>
    public class RegisterUsingTemplatesController : ApiController
    {
        /// <summary>
        /// Define the Notification Hubs client.
        /// </summary>
        private readonly NotificationHubClient _hubClient;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUsingTemplatesController"/> class.
        /// </summary>
        public RegisterUsingTemplatesController()
        { 
            // Create the client in the constructor.
             _hubClient = NotificationHubClient.CreateClientFromConnectionString(Constants.BackEndConnectionString, Constants.HubName);
        }

        /// <summary>
        /// Posts the specified registration call.
        /// </summary>
        /// <param name="registrationCall">The registration call.</param>
        /// <returns>The Task&lt;RegistrationDescription&gt;.</returns>
        public async Task<RegistrationDescription> PostAsync([FromBody] JObject registrationCall)
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
                    // Note: the tags can be from the device or from the backEnd storage, 
                    // it depends from the application requirements and in this case are used the installationid and the userName as example
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
                // Note: the tags can be from the device or from the backEnd storage, 
                // it depends from the application requirements and in this case are used the installationid and the userName as example
                string template;
                switch (platform)
                {
                        // in this case when we register the device we define the template for the target
                    case "windows":
                        template = new XElement("toast",
                           new XElement("visual",
                               new XElement("binding",
                                   new XAttribute("template", "ToastText01"),
                                   new XElement("text",
                                       new XAttribute("id", "1"),
                                       "$(message)")))).ToString(SaveOptions.DisableFormatting);
                        await _hubClient.CreateWindowsTemplateRegistrationAsync(channelUri, template, new string[] { installationId, userName });
                        break;
                    case "android":
                        template = new JObject(
                                new JProperty("data", new JObject(new JProperty("message", "$(message)"))))
                                .ToString(Newtonsoft.Json.Formatting.None);
                        registration = await _hubClient.CreateGcmTemplateRegistrationAsync(registrationId, template, new[] { installationId, userName });
                        break;
                    case "ios":
                        template = new JObject(
                               new JProperty("aps", new JObject(new JProperty("alert", "$(message)"))),
                               new JProperty("inAppMessage", "$(message)"))
                               .ToString(Newtonsoft.Json.Formatting.None);
                        await _hubClient.CreateAppleTemplateRegistrationAsync(deviceToken, template, new string[] { installationId, userName });
                        break;
                }
            }

            // Send out a test notification.
            await SendNotificationAsync(string.Format("Test notification for {0}", userName), userName);

            return registration;
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="notificationText">The notification text.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>Task.</returns>
        private async Task SendNotificationAsync(string notificationText, string tag)
        {
            // because the template was defined when the device was registered, at this moment we only need to send
            // the pairs key/value for fill the template
            var notification = new Dictionary<string, string> { { "message", notificationText } };
            await _hubClient.SendTemplateNotificationAsync(notification, tag);
        }

        /// <summary>
        /// Clears the registrations.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ClearRegistrationsAsync(string userName)
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
