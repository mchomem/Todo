namespace Todo.Application.Services.Interfaces;

public interface IUserService
{
    public Task CreateAsync(UserInsertDto entity);
    public Task DeleteAsync(int id);
    public Task<UserDto> GetAsync(int id);
    public Task<IEnumerable<UserDto>> GetAllAsync(UserFilter filter);
    public Task UpdateAsync(UserUpdateDto entity);
    public Task ChangePasswordAsync(UserChangePasswordDto userChangePassword);
}
