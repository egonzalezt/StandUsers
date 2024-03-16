namespace StandUsers.Infrastructure.MessageBroker.Options;

public class PublisherConfiguration
{
    public string UserRequestQueue { get; set; }
    public string UserOwnedBroadcastQueue { get; set; }
    public string UserNotificationsQueue { get; set; }
}
