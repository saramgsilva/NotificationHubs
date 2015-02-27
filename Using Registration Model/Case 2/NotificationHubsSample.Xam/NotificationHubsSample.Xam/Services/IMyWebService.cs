using System.Threading.Tasks;
namespace NotificationHubsSample.Xam.Services
{
    public interface IMyWebService
    {
        string PnsHandler { get; set; }
        string Platform { get; set; }
        Task<string> DoFakeLoginAsync(string userName, string password);
    }
}
