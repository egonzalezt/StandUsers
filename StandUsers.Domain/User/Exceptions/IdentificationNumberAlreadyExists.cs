namespace StandUsers.Domain.User.Exceptions;

using Domain.SharedKernel.Exceptions;

public class IdentificationNumberAlreadyExists : BusinessException
{
    public IdentificationNumberAlreadyExists() : base("IdentificationNumber in use")
    {
    }

    public IdentificationNumberAlreadyExists(string identificationNumber) : base($"IdentificationNumber {identificationNumber} in use")
    {
    }

    public IdentificationNumberAlreadyExists(string message, Exception innerException) : base(message, innerException)
    {
    }
}
