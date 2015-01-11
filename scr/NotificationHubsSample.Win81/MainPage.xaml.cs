using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NotificationHubsSample.Win81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
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

        /// <summary>
        /// Handles the OnClick event of the ButtonBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTags();
            NotificationService.CreateOrUpdateNotificationsAsync();
        }

        /// <summary>
        /// Updates the tags.
        /// </summary>
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
