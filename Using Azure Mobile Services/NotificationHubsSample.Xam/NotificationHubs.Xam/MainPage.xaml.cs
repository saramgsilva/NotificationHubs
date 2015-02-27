using NotificationHubs.Xam.ViewModel;
using Xamarin.Forms;

namespace NotificationHubs.Xam
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = ((ViewModelLocator) Application.Current.Resources["Locator"]).MainViewModel;
        }
    }
}
