using System;
using NotificationHubsSample.Xam.Services;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace NotificationHubsSample.Xam.Droid.Services
{
    public class SettingsService : ISettingsService
    {
        public const string RegistratrionIdKey = "RegistratrionIdKey";
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }
        public string RegistratrionId
        {
            get { return AppSettings.GetValueOrDefault(RegistratrionIdKey, Guid.NewGuid().ToString()); }
            set { AppSettings.AddOrUpdateValue(RegistratrionIdKey, value); }
        }
    }
}
