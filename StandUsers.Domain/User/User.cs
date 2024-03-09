namespace StandUsers.Domain.User;

using SharedKernel;
using StandUsers.Domain.User.Dtos;

public class User : Entity
{
    private User(
        Guid id,
        string name, 
        string email, 
        string direction,
        int identificationNumber
    ) : base(id)
    {
        Name = name;
        Email = email;
        Direction = direction;
        IdentificationNumber = identificationNumber;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Direction { get; private set; }
    public int IdentificationNumber { get; private set; }

    public static User Build(UserDto userDto)
    {
        var id = Guid.NewGuid();
        return new User(id, userDto.Name, userDto.Email.ToLower(), userDto.Direction, userDto.IdentificationNumber);
    }
}
