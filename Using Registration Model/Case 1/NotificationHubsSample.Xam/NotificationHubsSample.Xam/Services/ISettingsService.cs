using System.Collections.Generic;

namespace NotificationHubsSample.Xam.Services
{
    public interface ISettingsService
    {
        void Init(object context);
        List<string> Tags { get; set; }
    }
}
