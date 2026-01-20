namespace Todo.Domain.Exceptions.User;

public class UserAlreadyExistsException : UserException
{
    public UserAlreadyExistsException(string message = DefaultMessages.UserAlreadyExists) : base(message) { }
}
