namespace Todo.Domain.Exceptions.User;

public class UserException : BusinessException
{
    public UserException(string message) : base(message) { }    
}
