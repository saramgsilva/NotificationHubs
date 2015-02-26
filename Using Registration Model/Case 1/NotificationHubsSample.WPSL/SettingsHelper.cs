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
    }
}
