namespace Todo.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUserPictureRepository _userPictureRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository,
        ITodoItemRepository todoItemRepository,
        IUserPictureRepository userPictureRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _todoItemRepository = todoItemRepository;
        _userPictureRepository = userPictureRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(UserInsertDto entity)
    {
        var exists = await _userRepository.CheckIfExists(x => x.Login.Equals(entity.Login));

        if (exists)
            throw new UserAlreadyExistsException();

        var cypheredPassword = CypherHelper.Encrypt(entity.Password);
        var newUser = new User(entity.Name, entity.Login, cypheredPassword);

        await _userRepository.CreateAsync(newUser);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
            throw new UserNotFoundException();

        Expression<Func<TodoItem, bool>> expressionFilter = x => x.CreatedByID == id;

        var todos = await _todoItemRepository.GetAllAsync(expressionFilter);

        if (todos.Any())
        {
            await _todoItemRepository.DeleteByCreatedUserIdAsync(id);
        }

        if(user.Picture != null)
        {
            await _userPictureRepository.DeleteAsync(user.Picture);
        }

        await _userRepository.DeleteAsync(user);
    }

    public async Task<UserDto> GetAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
            throw new UserNotFoundException();

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(UserFilter filter)
    {
        Expression<Func<User, bool>> expressionFilter =
            x => (
                (!filter.UserId.HasValue || x.UserID == filter.UserId.Value)
                && (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name))
                && (string.IsNullOrEmpty(filter.Login) || x.Login == filter.Name)
                && x.IsActive
            );

        IEnumerable<User> users = await _userRepository.GetAllAsync(expressionFilter);

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task UpdateAsync(UserUpdateDto entity)
    {
        var user = await _userRepository.GetAsync(entity.UserID);

        if (user is null)
            throw new UserNotFoundException();

        UserPicture userPicture;

        if (user.Picture != null)
        {
            userPicture = await _userPictureRepository.GetAsync(user.Picture.UserPictureID);
        }
        else
        {
            userPicture = new UserPicture(entity.Picture, user.UserID, user);
        }

        userPicture.Update(entity.Picture);

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
        var user = await _userRepository.GetAsync(userChangePassword.UserID);

        if (user is null)
            throw new UserNotFoundException();

        userChangePassword.CurrentPassword = CypherHelper.Encrypt(userChangePassword.CurrentPassword);

        if (user.Password != userChangePassword.CurrentPassword)
            throw new UserPasswordsNotMatchException();

        var newPassword = CypherHelper.Encrypt(userChangePassword.NewPassword);
        user.ChangePassword(newPassword);

        await _userRepository.ChangePasswordAsync(user);
    }
}
