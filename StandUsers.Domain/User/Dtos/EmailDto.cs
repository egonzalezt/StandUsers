namespace StandUsers.Domain.User.Dtos;

using System.ComponentModel.DataAnnotations;

public class EmailDto
{
    [Required]
    [EmailAddress]
    public string Value { get; set; }
}
