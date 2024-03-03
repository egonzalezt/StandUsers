namespace StandUsers.Domain.User;

using SharedKernel;

public class User : Entity
{
    private User(
        Guid id,
        string name, 
        string email, 
        string identificationNumber
    ) : base(id)
    {
        Name = name;
        Email = email;
        IdentificationNumber = identificationNumber;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string IdentificationNumber { get; private set; }

    public static User Build(string name, string email, string identificationNumber)
    {
        var id = Guid.NewGuid();
        return new User(id, name, email.ToLower(), identificationNumber);
    }
}
