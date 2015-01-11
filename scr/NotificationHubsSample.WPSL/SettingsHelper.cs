using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace NotificationHubsSample
{
    /// <summary>
    /// Define the SettingsHelper.
    /// </summary>
    public static class SettingsHelper
    {
        private static readonly ApplicationDataContainer LocalSettings;
        private const string TagsKey = "Tags";
        private const string ChannelUriKey = "ChannelUri";
        private const string RegistrationIdKey = "RegistrationId";

        /// <summary>
        /// Initializes static members of the <see cref="SettingsHelper"/> class.
        /// </summary>
        static SettingsHelper()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public static List<string> Tags
        {
            get
            {
                if (LocalSettings.Values.ContainsKey(TagsKey))
                {
                    var tags = LocalSettings.Values[TagsKey].ToString();
                    return tags.Split(',').ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    LocalSettings.Values.Remove(TagsKey);
                }
                else
                {
                    LocalSettings.Values[TagsKey] = string.Join(",", value.ToArray());
                }
            }
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public static void RemoveAll()
        {
            LocalSettings.Values.Remove(ChannelUriKey);
            LocalSettings.Values.Remove(RegistrationIdKey);
        }

        /// <summary>
        /// Gets or sets the channel URI.
        /// </summary>
        /// <value>The channel URI.</value>
        public static string ChannelUri
        {
            get
            {
                if (LocalSettings.Values.ContainsKey(ChannelUriKey))
                {
                    return LocalSettings.Values[ChannelUriKey].ToString();
                }
                return string.Empty;
            }
            set
            {
                LocalSettings.Values[ChannelUriKey] = value;
            } 
        }

        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>The registration identifier.</value>
        public static string RegistrationId
        {
            get
            {
                if (LocalSettings.Values.ContainsKey(RegistrationIdKey))
                {
                    return LocalSettings.Values[RegistrationIdKey].ToString();
                }
                return string.Empty;
            }
            set
            {
                LocalSettings.Values[RegistrationIdKey] = value;
            }
        }
    }
}
