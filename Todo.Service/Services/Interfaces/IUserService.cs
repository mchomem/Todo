﻿using Todo.Domain.Dtos;
using Todo.Domain.Entities;

namespace Todo.Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateAsync(User entity);
        public Task DeleteAsync(User entity);
        public Task<User> DetailsAsync(User entity);
        public Task<IEnumerable<User>> RetrieveAsync(User entity);
        public Task UpdateAsync(User entity);
        public Task<UserDto> AuthenticateAsync(string login, string password);
        public Task ChangePasswordAsync(User entity, string newPassword);
    }
}
