using System;
using Xamarin.Forms;

namespace NotificationHubsSample.Views
{
    /// <summary>
    /// Define the RegisterPage.
    /// </summary>
    public class RegisterPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterPage"/> class.
        /// </summary>
        /// <param name="RegisterWithGcm">The register with GCM.</param>
        public RegisterPage(Action RegisterWithGcm)
        {
            Title = "Register in Notification Hubs";

            if (RegisterWithGcm != null)
            {
                RegisterWithGcm();
            }
        }
    }
}