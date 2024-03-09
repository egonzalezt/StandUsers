namespace StandUsers.Workers.Exceptions;

using StandUsers.Domain.SharedKernel.Exceptions;

internal class HeaderNotFoundException : BusinessException
{
    public HeaderNotFoundException() : base()
    {
    }

    public HeaderNotFoundException(string message) : base($"Header {message} not found")
    {
    }

    public HeaderNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
