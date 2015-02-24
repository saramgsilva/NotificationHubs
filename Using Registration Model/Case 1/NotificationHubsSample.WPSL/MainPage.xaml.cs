using System.Windows;
using System.Windows.Navigation;

namespace NotificationHubsSample
{
    public sealed partial class MainPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
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
                ToggleSwitchNews.IsChecked = true;
            }
            if (tags.Contains("sports"))
            {
                ToggleSwitchSports.IsChecked = true;
            }
            if (tags.Contains("music"))
            {
                ToggleSwitchMusic.IsChecked = true;
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
            App.NotificationService.CreateOrUpdateNotificationsAsync();
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
            if (ToggleSwitchNews.IsChecked.HasValue && ToggleSwitchNews.IsChecked.Value)
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
            if (ToggleSwitchSports.IsChecked.HasValue && ToggleSwitchSports.IsChecked.Value)
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
            if (ToggleSwitchMusic.IsChecked.HasValue && ToggleSwitchMusic.IsChecked.Value)
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