namespace Todo.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {        
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<UserDto> AuthenticateAsync(string login, string password)
    {
        var cypheredPassword = CypherHelper.Encrypt(password);

        var user = await _userRepository.AuthenticateAsync(login, cypheredPassword);

        if (user is null)
            throw new Exception("User not found.");

        var token = _tokenService.Generate(user.Name);

        // TODO: user mapping as Mapster or AutoMapper
        return new UserDto()
        {
            UserID = user.UserID!.Value,
            Name = user.Name,
            Picture = user.Picture?.Picture,
            IsActive = user.IsActive!.Value,
            Token = token
        };
    }
}
