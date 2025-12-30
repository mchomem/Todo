namespace Todo.Application.Services;

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
        var user = await _userRepository.DetailAsync(entity);
        return user;
    }

    public async Task<IEnumerable<User>> RetrieveAsync(User entity)
    {
        var users = await _userRepository.RetrieveAsync(entity);
        return users;
    }

    public async Task UpdateAsync(User entity)
    {
        entity.Password = CypherHelper.Encrypt(entity.Password);
        await _userRepository.UpdateAsync(entity);
    }

    public async Task<UserDto> AuthenticateAsync(string login, string password)
    {
        var user = await _userRepository
           .AuthenticateAsync(new User()
           {
               Login = login,
               Password = CypherHelper.Encrypt(password)
           });

        if (user is null)
            throw new Exception("User not found.");

        return new UserDto()
        {
            UserID = user.UserID!.Value,
            Name = user.Name,
            Picture = user.Picture?.Picture,
            IsActive = user.IsActive!.Value
        };
    }

    public async Task ChangePasswordAsync(User entity, string newPassword)
    {
        var user = await _userRepository.DetailAsync(entity);

        if (user is null)
            throw new Exception("User not found.");

        user.Update(newPassword);

        await _userRepository.ChangePasswordAsync(user, newPassword);
    }
}
