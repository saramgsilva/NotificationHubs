using System.Diagnostics;
using WindowsAzure.Messaging;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.Xam.Services;
using System;

namespace NotificationHubsSample.Xam.iOS.Services
{
    public class NotificationHubsService : INotificationHubsService
    {
        private readonly SBNotificationHub _hub;

        public NotificationHubsService()
        {
            // register device in notification hubs
            _hub = new SBNotificationHub(Constants.ConnectionString, Constants.HubName);
        }

        public string PnsHandler { get; set; }

        public void Init(object context)
        {
           
        }

        public void RegisterOrUpdate()
        {
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            // create tags if you want
			var tags = new NSSet(settingsService.Tags.ToArray());
           

			_hub.UnregisterAllAsync (PnsHandler, (error) => {
				if (error != null) 
				{
					Console.WriteLine("Error calling Unregister: {0}", error.ToString());
					return;
				} 

				_hub.RegisterNativeAsync(PnsHandler, tags, (errorCallback) => {
					if (errorCallback != null)
						Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
				});
			});
        }
    }
}