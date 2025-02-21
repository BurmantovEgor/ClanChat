﻿using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace ClanChat.Abstractions.User
{
    public interface IUserRepository
    {
        Task<UserEntity> FindByNameAsync(string username);
        Task<bool> CheckPasswordAsync(UserEntity user, string password);
        Task<IdentityResult> CreateAsync(UserEntity user, string password);
    }
}
