﻿namespace StandUsers.Domain.User.Repositories;

public interface IUserQueryRepository
{
    Task CreateAsync(User user);
}