using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Preferences;
using NotificationHubsSample.Xam.Services;

namespace NotificationHubsSample.Xam.Droid.Services
{
    public class SettingsService : ISettingsService
    {
        private ISharedPreferences _sharedPreferences;
        private const string TagsKey = "Tags";

        public void Init(object context)
        {
            _sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context as Context);
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public List<string> Tags
        {
            get
            {
                //if (_sharedPreferences.Contains(TagsKey))
                //{
                //    var tagsValue = _sharedPreferences.GetString(TagsKey, string.Empty);
                //    return tagsValue.Split(',').ToList();
                //}
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    var editor = _sharedPreferences.Edit();
                    editor.Remove(TagsKey);
                    editor.Apply();
                }
                else
                {
                    ISharedPreferencesEditor editor = _sharedPreferences.Edit();
                    editor.PutString(TagsKey, string.Join(",", value.ToArray()));
                    editor.Apply();
                }
            }
        }
    }
}
