namespace StandUsers.Domain.Centralizer.Dtos;

using StandUsers.Domain.SharedKernel;
using System.Text.Json.Serialization;
using User;
public class CreateUserDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("operatorId")]
    public int OperatorId { get; set; }
    [JsonPropertyName("operatorName")]
    public string OperatorName { get; set; }

    public static CreateUserDto Build(User user, OperatorIdentification providerIdentification)
    {
        return new CreateUserDto
        {
            Id = user.IdentificationNumber,
            Name = user.Name,
            Address = user.Direction,
            Email = user.Email,
            OperatorId = providerIdentification.Id,
            OperatorName = providerIdentification.Name,
        };
    }
}
