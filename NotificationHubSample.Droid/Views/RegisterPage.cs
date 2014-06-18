using System;
using Xamarin.Forms;

namespace NotificationHubSample.Droid.Views
{
    public class RegisterPage : ContentPage
    {
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