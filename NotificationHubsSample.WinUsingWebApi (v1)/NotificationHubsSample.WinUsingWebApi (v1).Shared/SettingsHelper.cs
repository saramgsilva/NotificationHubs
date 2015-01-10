using System;
using Windows.Storage;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public class SettingsHelper
    {
        private const string ContainerName = "InstallationContainer";
        private const string KeyName = "InstallationId";

        /// <summary>
        /// Gets the local storage container.
        /// </summary>
        /// <returns>ApplicationDataContainer.</returns>
        private static ApplicationDataContainer GetLocalStorageContainer()
        {
            
            if (!ApplicationData.Current.LocalSettings
                .Containers.ContainsKey(ContainerName))
            {
                ApplicationData.Current.LocalSettings
                    .CreateContainer(ContainerName,
                    ApplicationDataCreateDisposition.Always);
            }
            return ApplicationData.Current.LocalSettings.Containers[ContainerName];
        }

        /// <summary>
        /// Gets the installation identifier.
        /// </summary>
        /// <returns>The System.String.</returns>
        public static string GetInstallationId()
        {
            var container = GetLocalStorageContainer();
            if (!container.Values.ContainsKey(KeyName))
            {
                container.Values[KeyName] = Guid.NewGuid().ToString();
            }
            return (string)container.Values[KeyName];
        }
    }
}
