﻿namespace StandUsers.Domain.User.Dtos;

public class UserOwnedDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public long IdentificationNumber { get; set; }
}
