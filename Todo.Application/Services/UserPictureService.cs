using Todo.Application.Services.Interfaces;
using Todo.Domain.Entities;
using Todo.Infra.Database.Repositories.Interfaces;

namespace Todo.Application.Services;

public class UserPictureService : IUserPictureService
{
    private readonly IUserPictureRepository _userPictureRepository;

    public UserPictureService(IUserPictureRepository userPictureRepository)
        => _userPictureRepository = userPictureRepository;

    public async Task CreateAsync(UserPicture entity)
        => await _userPictureRepository.CreateAsync(entity);

    public async Task DeleteAsync(UserPicture entity)
        => await _userPictureRepository.DeleteAsync(entity);

    public async Task<UserPicture> DetailsAsync(UserPicture entity)
        => await _userPictureRepository.DetailAsync(entity);

    public async Task<IEnumerable<UserPicture>> RetrieveAsync(UserPicture entity)
        => await _userPictureRepository.RetrieveAsync(entity);

    public async Task UpdateAsync(UserPicture entity)
        => await _userPictureRepository.UpdateAsync(entity);
}
