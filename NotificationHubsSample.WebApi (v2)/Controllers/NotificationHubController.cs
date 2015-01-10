using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json.Linq;
using NotificationHubsSample.Models;

namespace NotificationHubsSample.Controllers
{
    /// <summary>
    /// Define the NotificationHubController.
    /// </summary>
    public class NotificationHubController : ApiController
    {
        private readonly NotificationHubClient _notificationHubClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHubController"/> class.
        /// </summary>
        public NotificationHubController()
        {
            _notificationHubClient = NotificationHubClient.CreateClientFromConnectionString(Constants.BackEndConnectionString, Constants.HubName);
        }

        /// <summary>
        /// Registers the specified device update.
        /// </summary>
        /// <param name="deviceUpdate">The device update.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        [HttpPost]
        [Route("api/NotificationHub/Register")]
        public async Task<string> RegisterAsync(DeviceInfo deviceUpdate)
        {
            var isRegistrationIdExpired = false;

            var registrationId = string.IsNullOrEmpty(deviceUpdate.RegistrationId)
                ? await _notificationHubClient.CreateRegistrationIdAsync()
                : deviceUpdate.RegistrationId;

            isRegistrationIdExpired = await RegisterDeviceAsync(deviceUpdate, registrationId);

            if (!isRegistrationIdExpired)
            {
                // send a test notification
                await SendTestNotificationByTagsAsync(deviceUpdate);
                return registrationId;
            }

            // if the registration id is expired the process must be repeated
            registrationId = await _notificationHubClient.CreateRegistrationIdAsync();
            await RegisterDeviceAsync(deviceUpdate, registrationId);

            // send a test notification
            await SendTestNotificationByTagsAsync(deviceUpdate);

            return registrationId;
        }

        /// <summary>
        /// Uns the register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpPost]
        [Route("api/NotificationHub/UnRegister")]
        public async void UnRegisterAsync(string id)
        {
            await _notificationHubClient.DeleteRegistrationAsync(id);
        }

        /// <summary>
        /// register device as an asynchronous operation.
        /// </summary>
        /// <param name="deviceUpdate">The device update.</param>
        /// <param name="registrationId">The registration identifier.</param>
        /// <returns>The Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> RegisterDeviceAsync(DeviceInfo deviceUpdate, string registrationId)
        {
            bool? isRegistrationIdExpired = false;
            RegistrationDescription registration;
            switch (deviceUpdate.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "gcm":
                    registration = new GcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            registration.RegistrationId = registrationId;
           
            // the tags can be from the device or from the backEnd storage, it depends from the application requirements
            registration.Tags = new HashSet<string>(deviceUpdate.Tags);

            try
            {
                await _notificationHubClient.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException messagingException)
            {
                isRegistrationIdExpired = IsRegistrationIdExpired(messagingException);
                if (isRegistrationIdExpired == null)
                {
                    throw;
                }
            }

            return isRegistrationIdExpired.Value;
        }

        /// <summary>
        /// Determines whether [is registration identifier expired] [the specified e].
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns><c>true</c> if [is registration identifier expired] [the specified e]; otherwise, <c>false</c>.</returns>
        private static bool? IsRegistrationIdExpired(MessagingException e)
        {
            var webException = e.InnerException as WebException;
            if (webException != null && webException.Status == WebExceptionStatus.ProtocolError)
            {
                var response = webException.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Gone)
                {
                    return true;
                }
            }
            return null;
        }

        /// <summary>
        /// Sends the test notification by tags.
        /// </summary>
        /// <param name="deviceUpdate">The device update.</param>
        /// <returns>Task.</returns>
        private async Task SendTestNotificationByTagsAsync(DeviceInfo deviceUpdate)
        {
            if (deviceUpdate.Tags.Any())
            {
                foreach (var tag in deviceUpdate.Tags)
                {
                    await SendNotificationAsync(string.Format("Test notification for {0}", tag), tag);
                }
            }
            else
            {
                await SendNotificationAsync(string.Format("Test notification"), string.Empty);
            }
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
                // Create notifications for both Windows Store and iOS platforms.
                var toast = new XElement("toast",
                              new XElement("visual",
                                  new XElement("binding",
                                      new XAttribute("template", "ToastText01"),
                                      new XElement("text",
                                          new XAttribute("id", "1"),
                                          notificationText)))).ToString(SaveOptions.DisableFormatting);
                var alert = new JObject(
                                new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
                                new JProperty("inAppMessage", notificationText))
                                .ToString(Newtonsoft.Json.Formatting.None);

                var payload = new JObject(
                                    new JProperty("data", new JObject(new JProperty("message", notificationText))))
                                    .ToString(Newtonsoft.Json.Formatting.None);

                // Send a notification to the logged-in user on both platforms.
                var googleResult = await _notificationHubClient.SendGcmNativeNotificationAsync(payload, tag);

                var windowsResult = await _notificationHubClient.SendWindowsNativeNotificationAsync(toast, tag);

                var appleResult = await _notificationHubClient.SendAppleNativeNotificationAsync(alert, tag);
            }
            catch (ArgumentException ex)
            {
                // This is expected when an APNS registration doesn't exist.
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
