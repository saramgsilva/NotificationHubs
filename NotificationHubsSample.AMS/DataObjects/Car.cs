using Microsoft.WindowsAzure.Mobile.Service;

namespace NotificationHubsSample.AMS.DataObjects
{
    public class Car : EntityData
    {
        public string Name { get; set; }

        public bool IsElectric { get; set; }
    }
}