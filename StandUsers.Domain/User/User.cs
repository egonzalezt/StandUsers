namespace StandUsers.Domain.User;

using SharedKernel;
using Dtos;
using System.Text;
using System.Security.Cryptography;

public class User : Entity
{
    private User(
        Guid id,
        string name, 
        string email, 
        string direction,
        long identificationNumber
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
    public long IdentificationNumber { get; private set; }
    public bool GovCarpetaVerified { get; private set; } = false;
    public bool Active { get; private set; } = false;

    public static User Build(UserDto userDto)
    {
        var id = GenerateGuidFromUserIdentificationNumber(userDto.IdentificationNumber);
        return new User(id, userDto.Name, userDto.Email.ToLower(), userDto.Direction, userDto.IdentificationNumber);
    }
    
    public void ActivateUserByGovCarpeta()
    {
        base.SetUpdated();
        GovCarpetaVerified = true;
    }

    public void DeactivateGovCarpeta()
    {
        base.SetUpdated();
        GovCarpetaVerified = false;
    }

    public void Activate()
    {
        base.SetUpdated();
        Active = true;
    }

    public void DeActivateUser()
    {
        base.SetUpdated();
        Active = false;
    }

    private static Guid GenerateGuidFromUserIdentificationNumber(long identificationNumber)
    {
        byte[] userIdBytes = Encoding.UTF8.GetBytes(identificationNumber.ToString());
        byte[] hash = SHA1.HashData(userIdBytes);
        hash[6] = (byte)(hash[6] & 0x0F | 0x40);
        hash[8] = (byte)(hash[8] & 0x3F | 0x80);
        byte[] guidBytes = hash.Take(16).ToArray();

        return new Guid(guidBytes);
    }
}
