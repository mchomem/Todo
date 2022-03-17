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
        public async Task Create(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.UserPictures.Add(entity);
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                db.UserPictures.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<UserPicture> Details(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return await db.UserPictures
                    .Include(x => x.User)
                        .FirstOrDefaultAsync(x =>
                            (
                                (!entity.UserPictureID.HasValue || x.UserPictureID == entity.UserPictureID)
                                && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID == entity.PictureFromUserID)
                            )
                        );
            }
        }

        public async Task<IEnumerable<UserPicture>> Retrieve(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return await db.UserPictures
                    .Where(x =>
                    (
                        (!entity.UserPictureID.HasValue || x.UserPictureID.Value == entity.UserPictureID.Value)
                        && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID.Value == entity.PictureFromUserID.Value)
                    ))
                    .ToListAsync();
            }
        }

        public async Task Update(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                db.UserPictures.Update(entity);
                await db.SaveChangesAsync();
            }
        }
    }
}
