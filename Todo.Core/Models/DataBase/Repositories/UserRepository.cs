using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class UserRepository : ICrud<User>
    {
        public void Create(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Remove(entity);
                db.SaveChanges();
            }
        }

        public User Details(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return db.Users
                    .FirstOrDefault(x => x.UserID == entity.UserID);
            }
        }

        public List<User> Retrieve(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return db.Users
                    .Where(x =>
                    (
                        (!entity.UserID.HasValue || x.UserID.Value == entity.UserID.Value)
                        && (String.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (String.IsNullOrEmpty(entity.Login) || x.Name.Contains(entity.Login))
                        && (!entity.IsActive.HasValue || x.IsActive.Value == entity.IsActive.Value)
                    ))
                    .ToList();
            }
        }

        public void Update(User entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Update(entity);
                db.SaveChanges();
            }
        }

        public UserDto Authenticate(User entity)
        {
            // TODO use an Utils.Cypher in password.

            using (TodoContext db = new TodoContext())
            {
                User user = db.Users
                    .Where(x => x.Login == entity.Login
                           && x.Password == entity.Password
                           && x.IsActive.Value)
                    .FirstOrDefault();

                if (user == null)
                    return null;

                return this.GetDto(user);
            }
        }

        private UserDto GetDto(User user)
        {
            return new UserDto()
            {
                UserID = user.UserID.Value,
                Name = user.Name,
                IsActive = user.IsActive.Value
            };
        }
    }
}
