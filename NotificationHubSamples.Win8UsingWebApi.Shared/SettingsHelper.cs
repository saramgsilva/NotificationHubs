using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NotificationHubSamples.Win8UsingWebApi
{
    public class SettingsHelper
    {
        private static ApplicationDataContainer GetLocalStorageContainer()
        {
            if (!ApplicationData.Current.LocalSettings
                .Containers.ContainsKey("InstallationContainer"))
            {
                ApplicationData.Current.LocalSettings
                    .CreateContainer("InstallationContainer",
                    ApplicationDataCreateDisposition.Always);
            }
            return ApplicationData.Current.LocalSettings
                .Containers["InstallationContainer"];
        }

        public static string GetInstallationId()
        {
            var container = GetLocalStorageContainer();
            if (!container.Values.ContainsKey("InstallationId"))
            {
                container.Values["InstallationId"] = Guid.NewGuid().ToString();
            }
            return (string)container.Values["InstallationId"];
        }
    }
}
