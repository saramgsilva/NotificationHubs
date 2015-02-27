using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using NotificationHubsSample.Xam.Model;

namespace NotificationHubsSample.Xam.Services
{
    public class MyWebService : IMyWebService
    {
        private const string RegisterUrl = "https://notificationhubssamplewebapiv2.azurewebsites.net/api/NotificationHub/Register";

        public string PnsHandler { get; set; }
        public string Platform { get; set; }
        public async Task<string> DoFakeLoginAsync(string userName, string password)
        {
            string message;
            try
            {
                // Get the info that we need to request registration.
                var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
                var registrationId = settingsService.RegistratrionId;

                // Define a registration to pass in the body of the POST request.
                var deviceInfo = new DeviceInfo
                {
                    Platform = Platform,
                    Handle = PnsHandler,
                    RegistrationId = registrationId,
                    Tags = new List<string>()
                };

                // Create a client to send the HTTP registration request.
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(RegisterUrl));

                // Add the Authorization header with the basic login credentials.
                var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password)));
                request.Headers.Add("Authorization", auth);
                request.Content = new StringContent(JsonConvert.SerializeObject(deviceInfo), Encoding.UTF8, "application/json");
                
                // Send the registration request.
                var response = await client.SendAsync(request);

                // Get and display the response, either the registration ID
                // or an error message.
                message = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}