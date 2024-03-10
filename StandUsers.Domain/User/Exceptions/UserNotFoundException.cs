using StandUsers.Domain.SharedKernel.Exceptions;

namespace Exceptions;

public class UserNotFoundException : BusinessException
{
    public UserNotFoundException() : base("User not found")
    {
    }

    public UserNotFoundException(string id) : base($"User with Id {id} not found")
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
