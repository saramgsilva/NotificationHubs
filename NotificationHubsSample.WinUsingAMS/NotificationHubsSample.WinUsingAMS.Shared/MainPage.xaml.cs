using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using NotificationHubsSample.WinUsingAMS.Model;

namespace NotificationHubsSample.WinUsingAMS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
        /// Buttons the base on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            var carToInsert = new Car
            {
                Name = "Renault 4l",
                IsElectric = false
            };
            var table = App.NotificationHubsSampleAMSClient.GetTable<Car>();
            await table.InsertAsync(carToInsert);

            var dialog = new MessageDialog("Inserted.");
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }
    }
}
