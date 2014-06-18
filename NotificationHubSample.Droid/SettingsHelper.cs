using System.Collections.Generic;
using System.Linq;
using Android.Content;

namespace NotificationHubSample.Droid
{
    public static class SettingsHelper
    {
        public static ISharedPreferences SharedPreferences { get; set; }

        public static List<string> Tags
        {
            get
            {
                if (SharedPreferences.Contains("tags"))
                {
                    var tagsValue = SharedPreferences.GetString("tags", string.Empty);
                    return tagsValue.Split(',').ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    var editor = SharedPreferences.Edit();
                    editor.Remove("tags");
                    editor.Apply();
                }
                else
                {
                    ISharedPreferencesEditor editor = SharedPreferences.Edit();
                    editor.PutString("tags", string.Join(",", value.ToArray()));
                    editor.Apply();
                }
            }
        }

        public static string RegistrationId 
        {
            get
            {
                return SharedPreferences.GetString("registrationId", string.Empty);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var editor = SharedPreferences.Edit();
                    editor.PutString("registrationId", value);
                    editor.Apply();
                }
                else
                {
                    var editor = SharedPreferences.Edit();
                    editor.Remove("registrationId");
                    editor.Apply();
                }
            }
        }
    }
}
