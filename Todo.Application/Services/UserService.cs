namespace Todo.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUserPictureRepository _userPictureRepository;

    public UserService(IUserRepository userRepository, ITodoItemRepository todoItemRepository, IUserPictureRepository userPictureRepository)
    {
        _userRepository = userRepository;
        _todoItemRepository = todoItemRepository;
        _userPictureRepository = userPictureRepository;
    }

    public async Task CreateAsync(UserInsertDto entity)
    {
        if ((await _userRepository.GetAllAsync(new User() { Login = entity.Login })).Any())
            throw new Exception("This user is already being used");

        entity.Password = CypherHelper.Encrypt(entity.Password);

        // TODO: user mapping as Mapster or AutoMapper
        var user = new User()
        {
            Name = entity.Name,
            Login = entity.Login,
            Password = entity.Password,
            IsActive = true
        };

        await _userRepository.CreateAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetAsync(new User() { UserID = id });

        if (user is null)
            throw new Exception("User not found.");

        var todos = await _todoItemRepository.GetAllAsync(new TodoItem() { CreatedByID = id });

        if(todos.Any())
        {
            await _todoItemRepository.DeleteByCreatedUserIdAsync(id);
        }

        await _userPictureRepository.DeleteAsync(user.Picture);
        await _userRepository.DeleteAsync(user);
    }

    public async Task<UserDto> GetAsync(int id)
    {
        var user = await _userRepository.GetAsync(new User() { UserID = id });

        if (user is null)
            throw new Exception("User not found.");

        // TODO: user mapping as Mapster or AutoMapper
        UserDto userDto = new UserDto()
        {
            UserID = user.UserID.Value,
            Name = user.Name,
            Picture = user.Picture?.Picture,
            IsActive = user.IsActive.Value
        };

        return userDto;
    }

    public async Task<IEnumerable<User>> GetAllAsync(User entity)
    {
        var users = await _userRepository.GetAllAsync(entity);
        return users;
    }

    public async Task UpdateAsync(UserUpdateDto entity)
    {
        var user = await _userRepository.GetAsync(new User() { UserID = entity.UserID });

        if(user is null)
            throw new Exception("User not found.");

        UserPicture userPicture;

        if(user.Picture != null)
        {
            userPicture = await _userPictureRepository.GetAsync(new UserPicture() { UserPictureID = user.Picture.UserPictureID });
        }
        else
        {
            userPicture = new UserPicture()
            {
                PictureFromUserID = user.UserID,
                User = user
            };
        }

        userPicture.Picture = entity.Picture;

        // Do already exists the user picture?
        if (user.Picture is null)
        {
            await _userPictureRepository.CreateAsync(userPicture);
        }
        else
        {
            await _userPictureRepository.UpdateAsync(userPicture);
        }

        user.Update(entity.Name);

        await _userRepository.UpdateAsync(user);
    }

    public async Task ChangePasswordAsync(UserChangePasswordDto userChangePassword)
    {
        var user = await _userRepository.GetAsync(new User() { UserID = userChangePassword.UserID});

        if (user is null)
            throw new Exception("User not found.");

        userChangePassword.CurrentPassword = CypherHelper.Encrypt(userChangePassword.CurrentPassword);

        if (user.Password != userChangePassword.CurrentPassword)
            throw new Exception("Current password is incorrect.");

        var newPassword = CypherHelper.Encrypt(userChangePassword.NewPassword);
        user.ChangePassword(newPassword);

        await _userRepository.ChangePasswordAsync(user);
    }
}
