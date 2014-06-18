using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NotificationHubSample_WP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var tags = SettingsHelper.Tags;
            if (tags.Count > 0)
            {
                Button.Content = "Update";
            }
            if (tags.Contains("news"))
            {
                ToggleSwitchNews.IsOn = true;
            }
            if (tags.Contains("sports"))
            {
                ToggleSwitchSports.IsOn = true;
            }
            if (tags.Contains("music"))
            {
                ToggleSwitchMusic.IsOn = true;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateTags();
            NotificationService.CreateOrUpdateNotificationsAsync();
        }

        private void UpdateTags()
        {
            var tags = SettingsHelper.Tags;
            if (tags.Count > 0)
            {
                Button.Content = "Update";
            }
            if (ToggleSwitchNews.IsOn)
            {
                if (!tags.Contains("news"))
                {
                    tags.Add("news");
                }
            }
            else
            {
                tags.Remove("news");
            }
            if (ToggleSwitchSports.IsOn)
            {
                if (!tags.Contains("sports"))
                {
                    tags.Add("sports");
                }
            }
            else
            {
                tags.Remove("sports");
            }
            if (ToggleSwitchMusic.IsOn)
            {
                if (!tags.Contains("music"))
                {
                    tags.Add("music");
                }
            }
            else
            {
                tags.Remove("music");
            }
            SettingsHelper.Tags = tags;
        }
    }
}
