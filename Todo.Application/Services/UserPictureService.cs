namespace Todo.Application.Services;

public class UserPictureService : IUserPictureService
{
    private readonly IUserPictureRepository _userPictureRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserPictureService(IUserPictureRepository userPictureRepository, IUserRepository userRepository, IMapper mapper)
    {
        _userPictureRepository = userPictureRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(UserPicture entity)
    {
         await _userPictureRepository.CreateAsync(entity);
    }

    public async Task DeleteByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetAsync(userId);

        if (user is null)
            throw new UserNotFoundException();

        var userPicture = await _userPictureRepository.GetByUserId(userId);

        if (userPicture is null)
            throw new UserPictureNotFoundException();

        await _userPictureRepository.DeleteAsync(userPicture);
    }

    public async Task<UserPicture> GetAsync(UserPicture entity)
    {
        var userPicture = await _userPictureRepository.GetAsync(entity.UserPictureID);
        return userPicture;
    }

    public async Task<IEnumerable<UserPictureDto>> GetAllAsync(UserPictureFilter filter)
    {
        Expression<Func<UserPicture, bool>> expressionFilter =
            x => (
                (!filter.UserPictureID.HasValue || x.UserPictureID == filter.UserPictureID.Value)
                && (!filter.PictureFromUserID.HasValue || x.PictureFromUserID.Value == filter.PictureFromUserID.Value)
            );

        var userPicturies = await _userPictureRepository.GetAllAsync(expressionFilter);
        var userPicturiesDto = _mapper.Map<IEnumerable<UserPictureDto>>(userPicturies);
        return userPicturiesDto;
    }

    public async Task UpdateAsync(UserPicture entity)
    {
        await _userPictureRepository.UpdateAsync(entity);
    }
}
