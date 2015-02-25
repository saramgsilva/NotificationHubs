using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.WinUsingAMS.Model;

namespace NotificationHubs.Xam.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
           
            InsertCommand = new RelayCommand(InsertCar);
        }

        private async void InsertCar()
        {
            var amsClient = ServiceLocator.Current.GetInstance<IAMSClient>();
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
            var car = new Car
            {
                Name = "My yellow car"
            };
            car =await amsClient.InsertAsync(car);
            if (car != null)
            {
                await dialog.ShowMessageBox("The car was inserted","NH Sample");
            }
        }

        public ICommand InsertCommand { get; set; }
    }
}
