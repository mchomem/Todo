using Todo.Domain.Dtos;
using Todo.Domain.Entities;
using Todo.Infra.Repositories.Interfaces;
using Todo.Service.Helpers;
using Todo.Service.Services.Interfaces;

namespace Todo.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateAsync(User entity)
    {
        entity.Password = CypherHelper.Encrypt(entity.Password);
        await _userRepository.CreateAsync(entity);
    }

    public async Task DeleteAsync(User entity)
    {
        await _userRepository.DeleteAsync(entity);
    }

    public async Task<User> DetailsAsync(User entity)
    {
        User user = await _userRepository.DetailAsync(entity);

        return user;
    }

    public async Task<IEnumerable<User>> RetrieveAsync(User entity)
    {
        return await _userRepository.RetrieveAsync(entity);
    }

    public async Task UpdateAsync(User entity)
    {
        entity.Password = CypherHelper.Encrypt(entity.Password);

        await _userRepository.UpdateAsync(entity);
    }

    public async Task<UserDto> AuthenticateAsync(string login, string password)
    {
        return await _userRepository
            .AuthenticateAsync(new User()
            {
                Login = login,
                Password = CypherHelper.Encrypt(password)
            });
    }

    public async Task ChangePasswordAsync(User entity, string newPassword)
    {
        await _userRepository.ChangePasswordAsync(entity, newPassword);
    }
}
