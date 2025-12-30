namespace Todo.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoItemRepository _todoItemRepository;

    public UserService(IUserRepository userRepository, ITodoItemRepository todoItemRepository)
    {
        _userRepository = userRepository;
        _todoItemRepository = todoItemRepository;
    }

    public async Task CreateAsync(User entity)
    {
        entity.Password = CypherHelper.Encrypt(entity.Password);
        await _userRepository.CreateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.DetailAsync(new User() { UserID = id });

        if (user is null)
            throw new Exception("User not found.");

        var todos = await _todoItemRepository.RetrieveAsync(new TodoItem() { CreatedByID = id });

        if(todos.Any())
        {
            await _todoItemRepository.DeleteByCreatedUserIdAsync(id);
        }

        await _userRepository.DeleteAsync(user);
    }

    public async Task<User> DetailsAsync(User entity)
    {
        var user = await _userRepository.DetailAsync(entity);

        if (user is null)
            throw new Exception("User not found.");

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
        var cypheredPassword = CypherHelper.Encrypt(password);

        var user = await _userRepository.AuthenticateAsync(login, cypheredPassword);

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

        newPassword = CypherHelper.Encrypt(newPassword);
        user.Update(newPassword);

        await _userRepository.ChangePasswordAsync(user, newPassword);
    }
}
