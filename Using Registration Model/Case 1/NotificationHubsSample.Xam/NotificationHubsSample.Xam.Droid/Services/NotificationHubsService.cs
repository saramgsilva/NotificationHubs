using Android.Content;
using Gcm;
using NotificationHubsSample.Xam.Services;

namespace NotificationHubsSample.Xam.Droid.Services
{
    public class NotificationHubsService : INotificationHubsService
    {
        private Context _context;
        public string PnsHandler { get; set; }

        public void Init(object context)
        {
            _context = context as Context;
        }

        public void RegisterOrUpdate()
        {
            // Check to ensure everything's setup right
            GcmClient.CheckDevice(_context);
            GcmClient.CheckManifest(_context);

            // Register for push notifications
            GcmClient.Register(_context, Constants.SenderID);
        }
    }
}