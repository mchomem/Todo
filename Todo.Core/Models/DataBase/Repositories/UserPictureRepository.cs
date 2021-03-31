using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class UserPictureRepository : IUserPictureRepository
    {
        public void Create(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.UserPictures.Add(entity);
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                db.SaveChanges();
            }
        }

        public void Delete(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                db.UserPictures.Remove(entity);
                db.SaveChanges();
            }
        }

        public UserPicture Details(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return db.UserPictures
                    .Include(x => x.User)
                        .FirstOrDefault(x =>
                            (
                                (!entity.UserPictureID.HasValue || x.UserPictureID == entity.UserPictureID)
                                && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID == entity.PictureFromUserID)
                            )
                        );
            }
        }

        public List<UserPicture> Retrieve(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return db.UserPictures
                    .Where(x =>
                    (
                        (!entity.UserPictureID.HasValue || x.UserPictureID.Value == entity.UserPictureID.Value)
                        && (!entity.PictureFromUserID.HasValue || x.PictureFromUserID.Value == entity.PictureFromUserID.Value)
                    ))
                    .ToList();
            }
        }

        public void Update(UserPicture entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.User);
                db.Entry(entity.User).State = EntityState.Unchanged;
                db.UserPictures.Update(entity);
                db.SaveChanges();
            }
        }
    }
}
