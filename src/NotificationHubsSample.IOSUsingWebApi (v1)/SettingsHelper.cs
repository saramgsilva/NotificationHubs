using System;
using Refractored.Xam.Settings;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public class SettingsHelper
    {
        private const string InstallationIdKey = "InstallationId";

        /// <summary>
        /// Gets the installation identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetInstallationId()
        {
            var settings = new Settings();
            var installationId = settings.GetValueOrDefault(InstallationIdKey, string.Empty);

            if (string.IsNullOrEmpty(installationId))
            {
                installationId = Guid.NewGuid().ToString();
                settings.AddOrUpdateValue(InstallationIdKey, installationId);
            }
            return installationId;
        }
    }
}