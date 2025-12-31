namespace Todo.Application.Services.Interfaces;

public interface IUserService
{
    public Task CreateAsync(UserInsertDto entity);
    public Task DeleteAsync(int id);
    public Task<UserDto> GetAsync(int id);
    public Task<IEnumerable<User>> GetAllAsync(User entity); // TODO: change User to USerDto and User to UseFilter
    public Task UpdateAsync(UserUpdateDto entity);
    public Task<UserDto> AuthenticateAsync(string login, string password);
    public Task ChangePasswordAsync(User entity, string newPassword); // TODO: change to UserChangePasswordDto
}
