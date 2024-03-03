namespace StandUsers.Domain.User.Exceptions;

using Domain.SharedKernel.Exceptions;

public class EmailAlreadyExists : BusinessException
{
    public EmailAlreadyExists() : base("Email in use")
    {
    }

    public EmailAlreadyExists(string email) : base($"Email {email} in use")
    {
    }

    public EmailAlreadyExists(string message, Exception innerException) : base(message, innerException)
    {
    }
}
