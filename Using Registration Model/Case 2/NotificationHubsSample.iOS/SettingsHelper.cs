using System;
using Refractored.Xam.Settings;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public class SettingsHelper
    {
        private const string RegistrationIdKey = "RegistrationId";

        /// <summary>
        /// Gets the registration identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetRegistrationId()
        {
            var settings = new Settings();
            var installationId = settings.GetValueOrDefault(RegistrationIdKey, string.Empty);

            if (string.IsNullOrEmpty(installationId))
            {
                installationId = Guid.NewGuid().ToString();
                settings.AddOrUpdateValue(RegistrationIdKey, installationId);
            }
            return installationId;
        }
    }
}