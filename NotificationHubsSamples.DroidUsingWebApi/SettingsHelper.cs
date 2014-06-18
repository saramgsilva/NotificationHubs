using System;

namespace NotificationHubsSamples.DroidUsingWebApi
{
    public class SettingsHelper
    {

        public static Android.Content.ISharedPreferences SharedPreferences { get; set; }

        public static string GetInstallationId()
        {
            if (!SharedPreferences.Contains("InstallationId"))
            {
                var editor = SharedPreferences.Edit();
                editor.PutString("InstallationId",  Guid.NewGuid().ToString());
                editor.Apply();
            }
            return SharedPreferences.GetString("InstallationId", Guid.NewGuid().ToString());
        }
    }
}