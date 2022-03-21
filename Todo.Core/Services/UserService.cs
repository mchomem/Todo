using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly Models.DataBase.Repositories.Interfaces.IUserRepository _userRepository;

        public UserService(Models.DataBase.Repositories.Interfaces.IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(User entity)
        {
            await _userRepository.Create(entity);
        }

        public async Task DeleteAsync(User entity)
        {
            await _userRepository.Delete(entity);
        }

        public async Task<User> DetailsAsync(User entity)
        {
            return await _userRepository.Details(entity);
        }

        public async Task<IEnumerable<User>> RetrieveAsync(User entity)
        {
            return await _userRepository.Retrieve(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            await _userRepository.Update(entity);
        }

        public async Task<UserDto> AuthenticateAsync(string login, string password)
        {
            return await _userRepository.Authenticate(new User() { Login = login, Password = password });
        }

        public async Task ChangePasswordAsync(User entity, string newPassword)
        {
            await _userRepository.ChangePassword(entity, newPassword);
        }
    }
}
