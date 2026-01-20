namespace Todo.Domain.Exceptions.Authentication;

public class AuthenticationException : BusinessException
{
    public AuthenticationException(string message) : base(message) { }
}
