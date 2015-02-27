using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using NotificationHubsSample.Xam.Services;
using Xamarin.Forms;

namespace NotificationHubsSample.Xam
{
    public partial class App : Application
    {
        public App(ISettingsService settingsService, INotificationHubsService notificationHubsService)
        {
            InitializeComponent();
            
            SimpleIoc.Default.Register<ISettingsService>(() => settingsService);
            SimpleIoc.Default.Register<INotificationHubsService>(() => notificationHubsService);

            // define the navigation
            var nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            // define the dialog service
            var dialog = new DialogService();
            SimpleIoc.Default.Register<IDialogService>(() => dialog);

            var navPage = new NavigationPage(new MainPage());

            // init services
            nav.Initialize(navPage);
            dialog.Initialize(navPage);

            MainPage = navPage;
        }
    }
}
