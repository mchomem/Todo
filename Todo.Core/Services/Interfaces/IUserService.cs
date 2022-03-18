using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Services.Interfaces
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
