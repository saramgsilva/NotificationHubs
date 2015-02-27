namespace NotificationHubsSample.Xam.Services
{
    public interface INotificationHubsService
    {
        string PnsHandler { get; set; }

        void Init(object context);
        void RegisterOrUpdate();
    }
}
