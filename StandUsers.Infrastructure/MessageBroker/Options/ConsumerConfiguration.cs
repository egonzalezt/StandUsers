namespace StandUsers.Infrastructure.MessageBroker.Options;

public class ConsumerConfiguration
{
    public string CreateUserQueue { get; set; }
    public string UserReplyQueue { get; set; }
    public string BebopUserTransferRequestQueue { get; set; }
}
