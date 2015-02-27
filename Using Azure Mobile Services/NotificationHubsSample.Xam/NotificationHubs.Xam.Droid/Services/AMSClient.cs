using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NotificationHubs.Xam.Services;
using NotificationHubsSample.WinUsingAMS.Model;
using NotificationHubsSample;

namespace NotificationHubs.Xam.Droid.Services
{
    public class AMSClient : IAMSClient
    {
        public static MobileServiceClient Client = new MobileServiceClient(Constants.AMSEndpoint, Constants.AMSKey);

        public async Task<Car> InsertAsync(Car car)
        {
            var table = Client.GetTable<Car>();
            await table.InsertAsync(car);
            return car;
        }

        public async Task RegisterNativateAsync(string pnsHandler)
        {
            // Register for push with Mobile Services
            var tags = new List<string> { "userId" };

            await Client.GetPush().RegisterNativeAsync(pnsHandler);

            // To register devices using tags
            // var tags = new List<string>();
            // Define the tags before register. Is possible to provide the userId and in backend
            // modify and use the tags based in the usedId (in this case tags should be stored in backend by usedId)
            //await Client.GetPush().RegisterNativeAsync(pnsHandler, tags);
        }
    }
}