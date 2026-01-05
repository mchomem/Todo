namespace Todo.Application.Services.Interfaces;

public interface IAuthService
{
    public Task<UserDto> AuthenticateAsync(string login, string password);
}
