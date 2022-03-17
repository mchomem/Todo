using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task Create(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Add(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> Details(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return await db.Users
                    .Include(x => x.Picture)
                        .FirstOrDefaultAsync(x => x.UserID == entity.UserID);
            }
        }

        public async Task<IEnumerable<User>> Retrieve(User entity)
        {
            // TODO: apply paggination with take and skip and a object to represent a paggination.
            using (TodoContext db = new TodoContext())
            {
                return await db.Users
                    .Where(x =>
                    (
                        (!entity.UserID.HasValue || x.UserID.Value == entity.UserID.Value)
                        && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (string.IsNullOrEmpty(entity.Login) || x.Name.Contains(entity.Login))
                        && (string.IsNullOrEmpty(entity.Password) || x.Password == entity.Password)
                        && (!entity.IsActive.HasValue || x.IsActive.Value == entity.IsActive.Value)
                    ))
                    .ToListAsync();
            }
        }

        public async Task Update(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                if (entity.Picture != null)
                {
                    db.UserPictures.Attach(entity.Picture);
                    db.Entry(entity.Picture).State = EntityState.Unchanged;
                }

                db.Users.Update(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<UserDto> Authenticate(User entity)
        {
            // TODO: use an Utils.Cypher in password.
            using (TodoContext db = new TodoContext())
            {
                User user = await db.Users
                    .Include(x => x.Picture)
                    .Where(x => x.Login == entity.Login
                           && x.Password == entity.Password
                           && x.IsActive.Value)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return null;

                return new UserDto()
                {
                    UserID = user.UserID.Value,
                    Name = user.Name,
                    Picture = user.Picture?.Picture,
                    IsActive = user.IsActive.Value
                };
            }
        }

        public async Task ChangePassword(User entity, string newPassword)
        {
            using (TodoContext db = new TodoContext())
            {
                User user = ((IEnumerable<User>)await this.Retrieve(entity)).FirstOrDefault();

                if (user == null)
                    throw new Exception("Incorrect user or password.");

                user.Password = newPassword;
                await this.Update(user);
            }
        }
    }
}
