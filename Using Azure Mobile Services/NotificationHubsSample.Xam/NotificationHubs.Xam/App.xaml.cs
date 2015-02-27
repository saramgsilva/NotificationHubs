using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using NotificationHubs.Xam.Services;
using Xamarin.Forms;

namespace NotificationHubs.Xam
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // The root page of your application
            MainPage = new MainPage();

            // define the navigation
            var nav = new NavigationService();
            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }

            // define the dialog service
            var dialog = new DialogService();
            if (!SimpleIoc.Default.IsRegistered<IDialogService>())
            {
                SimpleIoc.Default.Register<IDialogService>(() => dialog);
            }

            var navPage = new NavigationPage(new MainPage());

            // init services
            nav.Initialize(navPage);
            dialog.Initialize(navPage);

            MainPage = navPage;
        }
    }
}
