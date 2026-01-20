namespace Todo.Domain.Exceptions.UserPicture;

public class UserPictureNotFoundException : UserPictureException
{
    public UserPictureNotFoundException(string message = DefaultMessages.UserPictureNotFound) : base(message) { }
}
