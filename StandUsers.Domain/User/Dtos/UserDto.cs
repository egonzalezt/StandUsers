namespace StandUsers.Domain.User.Dtos;

using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Direction { get; set; }

    [Required]
    public int IdentificationNumber { get; set; }
}