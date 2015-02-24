using System.Collections.Generic;

namespace NotificationHubsSample.Model
{
    public class DeviceInfo
    {
        public string RegistrationId { get; set; }
        public string Platform { get; set; }
        public string Handle { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}