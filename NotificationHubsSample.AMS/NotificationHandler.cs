
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;

namespace NotificationHubsSample.AMS
{
    public class NotificationHandler : INotificationHandler
    {
        internal const string SomeTag = "sometag";

        public Task Register(ApiServices services, HttpRequestContext context, NotificationRegistration registration)
        {
            registration.Tags.Add(SomeTag);
            return Task.FromResult(true);
        }

        public Task Unregister(ApiServices services, HttpRequestContext context, string deviceId)
        {
            // do something befor unregister if your application needs
            return Task.FromResult(true);
        }
    }
}