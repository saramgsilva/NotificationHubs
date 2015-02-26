using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.Xam.Services;
using Xamarin.Forms;

namespace NotificationHubsSample.Xam
{
    public partial class MainPage : ContentPage
    {
        private readonly List<string> _tags;
        private readonly Switch _sportsSwitch;
        private readonly Switch _newsSwitch;
        private readonly Switch _musicSwitch;
        private readonly ISettingsService _settingService;

        public MainPage()
        {
            InitializeComponent();
            // todo change it to MVVM
            //BindingContext = ((ViewModelLocator) Application.Current.Resources["Locator"]).MainViewModel;
            var label = new Label
            {
                Text = "Set tags",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.Center
            };
           _settingService = ServiceLocator.Current.GetInstance<ISettingsService>();
            _tags = _settingService.Tags;

            var sportsLabel = new Label
            {
                Text = "Sports"
            };
            _sportsSwitch = new Switch
            {
                IsToggled = _tags.Contains("sports")
            };

            var newsLabel = new Label
            {
                Text = "News"
            };
            _newsSwitch = new Switch
            {
                IsToggled = _tags.Contains("news")
            };
            var musicLabel = new Label
            {
                Text = "Music"
            };
            _musicSwitch = new Switch
            {
                IsToggled = _tags.Contains("music")
            };
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
            _settingService.Tags = _tags;
            var notificationHubsService = ServiceLocator.Current.GetInstance<INotificationHubsService>();
            notificationHubsService.RegisterOrUpdate();
        }
    }
}
