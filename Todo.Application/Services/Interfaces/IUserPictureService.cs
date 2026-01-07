namespace Todo.Application.Services.Interfaces;

public interface IUserPictureService
{
    public Task CreateAsync(UserPicture entity);
    public Task DeleteByUserIdAsync(int userId);
    public Task<UserPicture> GetAsync(UserPicture entity);
    public Task<IEnumerable<UserPictureDto>> GetAllAsync(UserPictureFilter entity);
    public Task UpdateAsync(UserPicture entity);
}
