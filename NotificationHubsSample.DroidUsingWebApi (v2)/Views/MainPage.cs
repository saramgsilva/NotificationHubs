using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NotificationHubsSample.Model;
using Xamarin.Forms;

namespace NotificationHubsSample.Views
{
    public class MainPage : ContentPage
    {
        private const string RegisterUrl = "https://notificationhubssamplewebapiv2.azurewebsites.net/api/NotificationHub/Register";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            Title = "Login";

            User = new Entry { Placeholder = "User" };
            Password = new Entry{ Placeholder = "Password", IsPassword = true };

            InstId = new Label{ Text = "Instalation Id : N/D" };
            RegistrationId = new Label { Text = "Not registered!" };
            LoginButton = new Button {Text = "Login"};
            LoginButton.Clicked += Login_Click;


            var stackLayout = new StackLayout();
            stackLayout.Children.Add(InstId);
            stackLayout.Children.Add(RegistrationId);
            stackLayout.Children.Add(User);
            stackLayout.Children.Add(Password);
            stackLayout.Children.Add(LoginButton);

            Content = stackLayout;
        }

        /// <summary>
        /// Gets or sets the login button.
        /// </summary>
        /// <value>The login button.</value>
        public Button LoginButton { get; set; }

        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>The registration identifier.</value>
        public Label RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the inst identifier.
        /// </summary>
        /// <value>The inst identifier.</value>
        public Label InstId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public Entry Password { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public Entry User { get; set; }

        /// <summary>
        /// This method gets both an installation ID and channel for push notifications and sends it, 
        /// along with the device type, to the authenticated Web API method that creates a registration 
        /// in Notification Hubs.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> Instance containing the event data.</param>
        private async void Login_Click(object sender, EventArgs e)
        {
            // Get the info that we need to request registration.
#if __IOS__
            var registrationId = InstId.Text = SettingsHelper.GetRegistrationId();
#else
            var registrationId = InstId.Text = SettingsHelper.GetRegistrationId();
#endif
            var user = User.Text;
            var pwd = Password.Text;
            User.IsVisible = false;
            Password.IsVisible = false;
            LoginButton.IsVisible = false;
#if __IOS__
            // Define a registration to pass in the body of the POST request.
            var deviceInfo = new DeviceInfo()
            {
                Platform = "apns",
                Handle = "",
                RegistrationId = registrationId,
                Tags = new List<string>()
            };
#else
            // Define a registration to pass in the body of the POST request.
            var deviceInfo = new DeviceInfo()
            {
                Platform = "gcm",
                Handle = GcmService.RegistrationID,
                RegistrationId = registrationId,
                Tags = new List<string>()
            };
#endif

            // Create a client to send the HTTP registration request.
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(RegisterUrl));

            // Add the Authorization header with the basic login credentials.
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pwd));
            request.Headers.Add("Authorization", auth);
            request.Content = new StringContent(JsonConvert.SerializeObject(deviceInfo), Encoding.UTF8, "application/json");

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
            RegistrationId.Text = message;
        }
    }
}
