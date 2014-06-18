using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Java.Net;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NotificationHubsSamples.DroidUsingWebApi.Views
{
    public class MainPage : ContentPage
    {
        private string _registerUri = "https://<your service here>.azurewebsites.net/api/register";
        private string _registerUsingTemplatesUri = "https://<your service here>.azurewebsites.net/api/RegisterUsingTemplates";
      
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

        public Button LoginButton { get; set; }

        public Label RegistrationId { get; set; }

        public Label InstId { get; set; }

        public Entry Password { get; set; }

        public Entry User { get; set; }

        private async void Login_Click(object sender, EventArgs e)
        {
            // Get the info that we need to request registration.
            var installationId = InstId.Text = SettingsHelper.GetInstallationId();
            
            var user = User.Text;
            var pwd = Password.Text;
            User.IsVisible = false;
            Password.IsVisible = false;
            LoginButton.IsVisible = false;

            // Define a registration to pass in the body of the POST request.
            var registration = new Dictionary<string, string>()
            {
                { "platform", "android" },
                { "instId", installationId },
                { "registrationId", GcmService.RegistrationID }
            };

            // Create a client to send the HTTP registration request.
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_registerUsingTemplatesUri));

            // Add the Authorization header with the basic login credentials.
            var auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pwd));
            request.Headers.Add("Authorization", auth);
            request.Content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

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
