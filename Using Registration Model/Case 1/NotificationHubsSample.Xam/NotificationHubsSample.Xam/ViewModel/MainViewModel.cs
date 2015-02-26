using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.Xam.Services;

namespace NotificationHubsSample.Xam.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            RegisterOrUpdateCommand=new RelayCommand(RegisterOrUpdate);
        }

        public ICommand RegisterOrUpdateCommand { get; set; }

        private void RegisterOrUpdate()
        {
            var notificationHubsService = ServiceLocator.Current.GetInstance<INotificationHubsService>();
            notificationHubsService.RegisterOrUpdate();
        }
    }
}
