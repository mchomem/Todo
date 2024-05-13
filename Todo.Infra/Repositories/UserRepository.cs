using Microsoft.EntityFrameworkCore;
using Todo.Domain.Dtos;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;
using Todo.Infra.Repositories.Interfaces;

namespace Todo.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TodoContext _todoContext;

    public UserRepository(TodoContext todoContext)
    {
        _todoContext = todoContext;
    }

    public async Task CreateAsync(User entity)
    {
        _todoContext.Users.Add(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _todoContext.Users.Remove(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task<User> DetailAsync(User entity)
    {
        return await _todoContext.Users
            .Include(x => x.Picture)
                .FirstOrDefaultAsync(x => x.UserID == entity.UserID);
    }

    public async Task<IEnumerable<User>> RetrieveAsync(User entity)
    {
        // TODO: apply paggination with take and skip and a object to represent a paggination.
        return await _todoContext.Users
            .Where(x =>
            (
                (!entity.UserID.HasValue || x.UserID.Value == entity.UserID.Value)
                && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                && (string.IsNullOrEmpty(entity.Login) || x.Name.Contains(entity.Login))
                && (!entity.IsActive.HasValue || x.IsActive.Value == entity.IsActive.Value)
            ))
            .ToListAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        if (entity.Picture != null)
        {
            _todoContext.UserPictures.Attach(entity.Picture);
            _todoContext.Entry(entity.Picture).State = EntityState.Unchanged;
        }

        _todoContext.Users.Update(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task<UserDto> AuthenticateAsync(User entity)
    {
        User user = await _todoContext.Users
            .Include(x => x.Picture)
            .Where(x => x.Login == entity.Login
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

    public async Task ChangePasswordAsync(User entity, string newPassword)
    {
        User user = (await this.RetrieveAsync(entity)).FirstOrDefault();

        if (user == null)
            throw new Exception("Incorrect user or password.");

        user.Password = newPassword;
        await this.UpdateAsync(user);
    }
}
