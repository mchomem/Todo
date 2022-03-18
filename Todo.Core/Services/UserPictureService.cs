using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;
using Todo.Core.Services.Interfaces;

namespace Todo.Core.Services
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
            await _userPictureRepository.Create(entity);
        }

        public async Task DeleteAsync(UserPicture entity)
        {
            await _userPictureRepository.Delete(entity);
        }

        public async Task<UserPicture> DetailsAsync(UserPicture entity)
        {
            return await _userPictureRepository.Details(entity);
        }

        public async Task<IEnumerable<UserPicture>> RetrieveAsync(UserPicture entity)
        {
            return await _userPictureRepository.Retrieve(entity);
        }

        public async Task UpdateAsync(UserPicture entity)
        {
            await _userPictureRepository.Update(entity);
        }
    }
}
