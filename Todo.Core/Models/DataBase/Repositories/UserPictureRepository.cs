using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class UserPictureRepository : IUserPictureRepository
    {
        private readonly TodoContext _todoContext;

        public UserPictureRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task Create(UserPicture entity)
        {
            _todoContext.UserPictures.Add(entity);
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            await _todoContext.SaveChangesAsync();
        }

        public async Task Delete(UserPicture entity)
        {
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            _todoContext.UserPictures.Remove(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<UserPicture> Details(UserPicture entity)
        {
            return await _todoContext.UserPictures
                .Include(x => x.User)
                    .FirstOrDefaultAsync(x =>
                        (
                            (!entity.UserPictureID.HasValue || x.UserPictureID == entity.UserPictureID)
                            && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID == entity.PictureFromUserID)
                        )
                    );
        }

        public async Task<IEnumerable<UserPicture>> Retrieve(UserPicture entity)
        {
            return await _todoContext.UserPictures
                .Where(x =>
                (
                    (!entity.UserPictureID.HasValue || x.UserPictureID.Value == entity.UserPictureID.Value)
                    && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID.Value == entity.PictureFromUserID.Value)
                ))
                .ToListAsync();
        }

        public async Task Update(UserPicture entity)
        {
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            _todoContext.UserPictures.Update(entity);
            await _todoContext.SaveChangesAsync();
        }
    }
}
