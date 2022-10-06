using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;
using Todo.Infra.Repositories.Interfaces;

namespace Todo.Infra.Repositories
{
    public class UserPictureRepository : IUserPictureRepository
    {
        private readonly TodoContext _todoContext;

        public UserPictureRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task CreateAsync(UserPicture entity)
        {
            _todoContext.UserPictures.Add(entity);
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            await _todoContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserPicture entity)
        {
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            _todoContext.UserPictures.Remove(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<UserPicture> DetailAsync(UserPicture entity)
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

        public async Task<IEnumerable<UserPicture>> RetrieveAsync(UserPicture entity)
        {
            return await _todoContext.UserPictures
                .Where(x =>
                (
                    (!entity.UserPictureID.HasValue || x.UserPictureID.Value == entity.UserPictureID.Value)
                    && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID.Value == entity.PictureFromUserID.Value)
                ))
                .ToListAsync();
        }

        public async Task UpdateAsync(UserPicture entity)
        {
            _todoContext.Users.Attach(entity.User);
            _todoContext.Entry(entity.User).State = EntityState.Unchanged;
            _todoContext.UserPictures.Update(entity);
            await _todoContext.SaveChangesAsync();
        }
    }
}
