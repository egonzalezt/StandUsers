namespace StandUsers.Domain.User.Dtos;

using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public EmailDto Email { get; set; }

    [Required]
    public IdentificationDto IdentificationNumber { get; set; }
}