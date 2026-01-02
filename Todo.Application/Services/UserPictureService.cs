namespace Todo.Application.Services;

public class UserPictureService : IUserPictureService
{
    private readonly IUserPictureRepository _userPictureRepository;
    private readonly IUserRepository _userRepository;

    public UserPictureService(IUserPictureRepository userPictureRepository, IUserRepository userRepository)
    {
        _userPictureRepository = userPictureRepository;
        _userRepository = userRepository;
    }

    public async Task CreateAsync(UserPicture entity)
        => await _userPictureRepository.CreateAsync(entity);

    public async Task DeleteByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetAsync(new User { UserID = userId });

        if (user is null)
            throw new Exception("User not found.");

        var userPicture = await _userPictureRepository.GetAsync(new UserPicture { PictureFromUserID = userId });

        if (userPicture is null)
            throw new Exception("User picture not found.");

        await _userPictureRepository.DeleteAsync(userPicture);
    }

    public async Task<UserPicture> GetAsync(UserPicture entity)
        => await _userPictureRepository.GetAsync(entity);

    public async Task<IEnumerable<UserPicture>> GetAllAsync(UserPicture entity)
        => await _userPictureRepository.GetAllAsync(entity);

    public async Task UpdateAsync(UserPicture entity)
        => await _userPictureRepository.UpdateAsync(entity);
}
