using NotificationHubsSample.Xam.ViewModel;
using Xamarin.Forms;

namespace NotificationHubsSample.Xam
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = ((ViewModelLocator)Application.Current.Resources["Locator"]).MainViewModel;
        }
    }
}
