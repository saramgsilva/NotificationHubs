using System.Collections.Generic;
using System.Linq;
using NotificationHubsSample.Xam.Services;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace NotificationHubsSample.Xam.iOS.Services
{
    public class SettingsService : ISettingsService
    {
        private const string TagsKey = "Tags";

        public void Init(object context)
        {
           
        }

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public List<string> Tags
        {
            get
            {
                var tagsValue = AppSettings.GetValueOrDefault(TagsKey, string.Empty);
                return tagsValue.Split(',').ToList();
            }
            set
            {
                AppSettings.AddOrUpdateValue(TagsKey, string.Join(",", value.ToArray()));
            }
        }
    }
}
