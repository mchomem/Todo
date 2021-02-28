using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories.Interfaces
{
    public interface IUserRepository : ICrud<User>
    {
        public UserDto Authenticate(User entity);

        public void ChangePassword(User entity, string newPassword);
    }
}
