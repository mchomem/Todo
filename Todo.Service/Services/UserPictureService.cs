using Todo.Domain.Entities;
using Todo.Infra.Repositories.Interfaces;
using Todo.Service.Services.Interfaces;

namespace Todo.Service.Services
{
    public class UserPictureService : IUserPictureService
    {
        private readonly IUserPictureRepository _userPictureRepository;

        public UserPictureService(IUserPictureRepository userPictureRepository)
        {
            _userPictureRepository = userPictureRepository;
        }

        public async Task CreateAsync(UserPicture entity)
        {
            await _userPictureRepository.CreateAsync(entity);
        }

        public async Task DeleteAsync(UserPicture entity)
        {
            await _userPictureRepository.DeleteAsync(entity);
        }

        public async Task<UserPicture> DetailsAsync(UserPicture entity)
        {
            return await _userPictureRepository.DetailAsync(entity);
        }

        public async Task<IEnumerable<UserPicture>> RetrieveAsync(UserPicture entity)
        {
            return await _userPictureRepository.RetrieveAsync(entity);
        }

        public async Task UpdateAsync(UserPicture entity)
        {
            await _userPictureRepository.UpdateAsync(entity);
        }
    }
}
