namespace Todo.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserDto> AuthenticateAsync(string login, string password)
    {
        var cypheredPassword = CypherHelper.Encrypt(password);
        var user = await _userRepository.AuthenticateAsync(login, cypheredPassword);

        if (user is null)
            throw new Exception("User not found.");

        var userDto = _mapper.Map<UserDto>(user);
        userDto.Token = _tokenService.Generate(user.Name!);

        return userDto;
    }
}
