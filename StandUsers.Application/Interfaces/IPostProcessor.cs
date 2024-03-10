using StandUsers.Domain.SharedKernel;

namespace StandUsers.Application.Interfaces;

public interface IPostProcessor<in T> where T : class
{
    void NotifyCreation(T entity, string messageType, NotificationTypes notificationType);
    void NotifyDeletion(T entity, string messageType, NotificationTypes notificationType);
}
