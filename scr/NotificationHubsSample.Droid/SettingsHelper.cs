using System.Collections.Generic;
using System.Linq;
using Android.Content;

namespace NotificationHubsSample
{
    public static class SettingsHelper
    {
        private const string TagsKey = "Tags";
        private const string RegistrationIdKey = "RegistrationId";

        /// <summary>
        /// Gets or sets the shared preferences.
        /// </summary>
        /// <value>The shared preferences.</value>
        public static ISharedPreferences SharedPreferences { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public static List<string> Tags
        {
            get
            {
                if (SharedPreferences.Contains(TagsKey))
                {
                    var tagsValue = SharedPreferences.GetString(TagsKey, string.Empty);
                    return tagsValue.Split(',').ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    var editor = SharedPreferences.Edit();
                    editor.Remove(TagsKey);
                    editor.Apply();
                }
                else
                {
                    ISharedPreferencesEditor editor = SharedPreferences.Edit();
                    editor.PutString(TagsKey, string.Join(",", value.ToArray()));
                    editor.Apply();
                }
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
                return SharedPreferences.GetString(RegistrationIdKey, string.Empty);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var editor = SharedPreferences.Edit();
                    editor.PutString(RegistrationIdKey, value);
                    editor.Apply();
                }
                else
                {
                    var editor = SharedPreferences.Edit();
                    editor.Remove(RegistrationIdKey);
                    editor.Apply();
                }
            }
        }
    }
}
