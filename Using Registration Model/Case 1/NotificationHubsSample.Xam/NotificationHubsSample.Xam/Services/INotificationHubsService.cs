namespace NotificationHubsSample.Xam.Services
{
    public interface INotificationHubsService
    {
        void Init(object context);
        void RegisterOrUpdate();
    }
}
