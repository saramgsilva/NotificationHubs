using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NotificationHubsSample.Views
{
    /// <summary>
    /// Define the TagsPage.
    /// </summary>
    public class TagsPage: ContentPage
    {
        private List<string> _tags;
        private Switch _sportsSwitch;
        private Switch _newsSwitch;
        private Switch _musicSwitch;
        private Action RegisterWithGcm;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsPage"/> class.
        /// </summary>
        /// <param name="registerWithGcm">The register with GCM.</param>
        public TagsPage(Action registerWithGcm)
        {
            RegisterWithGcm = registerWithGcm;
            _tags = SettingsHelper.Tags;
            Title = "Notification Hub Sample";
            NavigationPage.SetHasNavigationBar(this, true);

            var label = new Label
            {
                Text = "Set tags",
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                XAlign = TextAlignment.Center
            };

            var sportsLabel = new Label { Text = "Sports" };
            _sportsSwitch = new Switch { IsToggled = _tags.Contains("sports") };

            var newsLabel = new Label { Text = "News" };
            _newsSwitch = new Switch { IsToggled = _tags.Contains("news") };

            var musicLabel = new Label { Text = "Music" };
            _musicSwitch = new Switch { IsToggled = _tags.Contains("music") };

            var button = new Button { Text = _tags.Count == 0 ? "Register" : "Update" };
            button.Clicked += ButtonClicked;
            var stackPanel = new StackLayout();

            stackPanel.Children.Add(label);
            stackPanel.Children.Add(sportsLabel);
            stackPanel.Children.Add(_sportsSwitch);
            stackPanel.Children.Add(newsLabel);
            stackPanel.Children.Add(_newsSwitch);
            stackPanel.Children.Add(musicLabel);
            stackPanel.Children.Add(_musicSwitch);
            stackPanel.Children.Add(button);

            Content = stackPanel;

            if (RegisterWithGcm != null)
            {
                RegisterWithGcm();
            }
        }

        /// <summary>
        /// Handles the Clicked event of the button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonClicked(object sender, System.EventArgs e)
        {

            if (_newsSwitch.IsToggled)
            {
                if (!_tags.Contains("news"))
                {
                    _tags.Add("news");
                }
            }
            else
            {
                _tags.Remove("news");
            }
            if (_sportsSwitch.IsToggled)
            {
                if (!_tags.Contains("sports"))
                {
                    _tags.Add("sports");
                }
            }
            else
            {
                _tags.Remove("sports");
            }
            if (_musicSwitch.IsToggled)
            {
                if (!_tags.Contains("music"))
                {
                    _tags.Add("music");
                }
            }
            else
            {
                _tags.Remove("music");
            }
            SettingsHelper.Tags = _tags;
            if (RegisterWithGcm != null)
            {
                RegisterWithGcm();
            }
        }
    }
}