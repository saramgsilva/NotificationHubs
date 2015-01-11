using System;
using Windows.Storage;

namespace NotificationHubsSample
{
    public class SettingsHelper
    {
        private static readonly ApplicationDataContainer LocalSettings;
        private const string KeyName = "RegistrationId";

        /// <summary>
        /// Initializes static members of the <see cref="SettingsHelper"/> class.
        /// </summary>
        static SettingsHelper()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        public static string GetRegistrationId()
        {
            if (!LocalSettings.Values.ContainsKey(KeyName))
            {
                LocalSettings.Values[KeyName] = Guid.NewGuid().ToString();
            }
            return (string)LocalSettings.Values[KeyName];
        }
    }
}
