namespace Todo.Domain.Exceptions.User;

public class UserPasswordsNotMatchException : UserException
{
    public UserPasswordsNotMatchException(string message = DefaultMessages.UserPasswordsNotMatch) : base(message) { }
}
