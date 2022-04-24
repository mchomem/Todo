using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;
using Todo.Core.Models.Utils;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _todoContext;

        public UserRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task Create(User entity)
        {
            entity.Password = Cypher.Encrypt(entity.Password);
            _todoContext.Users.Add(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            _todoContext.Users.Remove(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<User> Details(User entity)
        {
            User user = await _todoContext.Users
                .Include(x => x.Picture)
                    .FirstOrDefaultAsync(x => x.UserID == entity.UserID);

            user.Password = Cypher.Decrypt(user.Password);

            return user;
        }

        public async Task<IEnumerable<User>> Retrieve(User entity)
        {
            // TODO: apply paggination with take and skip and a object to represent a paggination.
            return await _todoContext.Users
                .Where(x =>
                (
                    (!entity.UserID.HasValue || x.UserID.Value == entity.UserID.Value)
                    && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                    && (string.IsNullOrEmpty(entity.Login) || x.Name.Contains(entity.Login))
                    && (string.IsNullOrEmpty(entity.Password) || x.Password == Cypher.Encrypt(entity.Password))
                    && (!entity.IsActive.HasValue || x.IsActive.Value == entity.IsActive.Value)
                ))
                .ToListAsync();
        }

        public async Task Update(User entity)
        {
            entity.Password = Cypher.Encrypt(entity.Password);

            if (entity.Picture != null)
            {
                _todoContext.UserPictures.Attach(entity.Picture);
                _todoContext.Entry(entity.Picture).State = EntityState.Unchanged;
            }

            _todoContext.Users.Update(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<UserDto> Authenticate(User entity)
        {
            User user = await _todoContext.Users
                .Include(x => x.Picture)
                .Where(x => x.Login == entity.Login
                       && x.Password == Cypher.Encrypt(entity.Password)
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

        public async Task ChangePassword(User entity, string newPassword)
        {
            User user = (await this.Retrieve(entity)).FirstOrDefault();

            if (user == null)
                throw new Exception("Incorrect user or password.");

            user.Password = newPassword;
            await this.Update(user);
        }
    }
}
