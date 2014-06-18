using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace NotificationHubSample_WP
{
    public static class SettingsHelper
    {
        private static readonly ApplicationDataContainer _localSettings;

        static SettingsHelper()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public static List<string> Tags
        {
            get
            {
                if (_localSettings.Values.ContainsKey("Tags"))
                {
                    var tags = _localSettings.Values["Tags"].ToString();
                    return tags.Split(',').ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _localSettings.Values.Remove("Tags");
                }
                else
                {
                    _localSettings.Values["Tags"] = string.Join(",", value.ToArray());
                }
            }
        }

        public static void RemoveAll()
        {
            _localSettings.Values.Remove("ChannelUri");
            _localSettings.Values.Remove("RegistrationId");
        }

        public static string ChannelUri
        {
            get
            {
                if (_localSettings.Values.ContainsKey("ChannelUri"))
                {
                    return _localSettings.Values["ChannelUri"].ToString();
                }
                return string.Empty;
            }
            set
            {
                _localSettings.Values["ChannelUri"] = value;
            } 
        }

        public static string RegistrationId
        {
            get
            {
                if (_localSettings.Values.ContainsKey("RegistrationId"))
                {
                    return _localSettings.Values["RegistrationId"].ToString();
                }
                return string.Empty;
            }
            set
            {
                _localSettings.Values["RegistrationId"] = value;
            }
        }
    }
}
