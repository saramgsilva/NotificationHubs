using System.Threading.Tasks;
using NotificationHubsSample.WinUsingAMS.Model;

namespace NotificationHubs.Xam.Services
{
    public interface IAMSClient
    {
        Task<Car> InsertAsync(Car car);
        Task RegisterNativateAsync(string pnsHandler);
    }
}
