using System;
using Windows.Storage;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public class SettingsHelper
    {
        private const string KeyName = "InstationId";
        private static readonly ApplicationDataContainer LocalSettings;
   
        /// <summary>
        /// Initializes static members of the <see cref="SettingsHelper"/> class.
        /// </summary>
        static SettingsHelper()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Gets the installation identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetInstallationId()
        {
            if (!LocalSettings.Values.ContainsKey(KeyName))
            {
                LocalSettings.Values[KeyName] = Guid.NewGuid().ToString();
            }
            return (string)LocalSettings.Values[KeyName];
        }
    }
}
