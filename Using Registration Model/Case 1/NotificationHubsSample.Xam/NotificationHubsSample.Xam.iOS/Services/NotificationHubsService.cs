using System.Diagnostics;
using WindowsAzure.Messaging;
using Foundation;
using NotificationHubsSample.Xam.Services;

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
            // create tags if you want
            var tags = new NSSet("news");
            _hub.RegisterNativeAsync(PnsHandler, tags, errorCallback =>
            {
                if (errorCallback != null)
                {
                    Debug.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                }
            });
        }
    }
}