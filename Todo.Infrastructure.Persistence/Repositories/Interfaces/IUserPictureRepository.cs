namespace Todo.Infrastructure.Persistence.Repositories.Interfaces;

public interface IUserPictureRepository : IRepository<UserPicture>
{
    Task<UserPicture> GetByUserId(int userId);
}
