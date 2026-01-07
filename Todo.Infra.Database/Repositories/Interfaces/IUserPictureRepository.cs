namespace Todo.Infra.Database.Repositories.Interfaces;

public interface IUserPictureRepository : IRepository<UserPicture>
{
    Task<UserPicture> GetByUserId(int userId);
}
