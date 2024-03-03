namespace StandUsers.Domain.User.Dtos;

using System.ComponentModel.DataAnnotations;

public class IdentificationDto
{

    [Required]
    [RegularExpression(@"^\d{5,10}$", ErrorMessage = "IdentificationNumber must have 5 y 10 digits.")]
    public string Value { get; set; }
}
