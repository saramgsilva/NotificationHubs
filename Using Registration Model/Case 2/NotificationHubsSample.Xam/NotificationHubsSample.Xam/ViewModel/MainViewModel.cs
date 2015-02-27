using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using NotificationHubsSample.Xam.Services;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace NotificationHubsSample.Xam.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            FakeLoginCommand = new RelayCommand(async()=>await DoFakeLoginAsync());
        }

        public ICommand FakeLoginCommand { get; set; }

        private async Task DoFakeLoginAsync()
        {
            var notificationHubsService = ServiceLocator.Current.GetInstance<IMyWebService>();

            //the password and the username must be equal
           var result = await notificationHubsService.DoFakeLoginAsync("myUser", "myUser");
           var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
           var message = string.Format("The registration result is {0}", result);
           await dialogService.ShowMessage(message, "Notification Hubs");
        }
    }
}
