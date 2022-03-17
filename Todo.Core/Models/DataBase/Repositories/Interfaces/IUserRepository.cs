using System.Threading.Tasks;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories.Interfaces
{
    public interface IUserRepository : ICrud<User>
    {
        public Task<UserDto> Authenticate(User entity);

        public Task ChangePassword(User entity, string newPassword);
    }
}
