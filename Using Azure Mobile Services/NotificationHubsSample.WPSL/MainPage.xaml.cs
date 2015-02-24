using System.Windows;
using NotificationHubsSample.WinUsingAMS.Model;

namespace NotificationHubsSample.WPSLUsingAMS
{
    public partial class MainPage
    {
        // Constructor
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

            MessageBox.Show("Inserted.");
        }
    }
}