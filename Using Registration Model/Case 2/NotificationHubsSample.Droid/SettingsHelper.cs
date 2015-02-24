using System;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public class SettingsHelper
    {
        private const string InstallationIdKey = "InstallationId";

        /// <summary>
        /// Gets or sets the shared preferences.
        /// </summary>
        /// <value>The shared preferences.</value>
        public static Android.Content.ISharedPreferences SharedPreferences { get; set; }

        /// <summary>
        /// Gets the installation identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetRegistrationId()
        {
            if (!SharedPreferences.Contains(InstallationIdKey))
            {
                var editor = SharedPreferences.Edit();
                editor.PutString(InstallationIdKey, Guid.NewGuid().ToString());
                editor.Apply();
            }
            return SharedPreferences.GetString(InstallationIdKey, Guid.NewGuid().ToString());
        }
    }
}