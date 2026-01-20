namespace Todo.Domain.Exceptions.Authentication;

public class AuthenticationPasswordOrAccountIncorrectException : AuthenticationException
{
    public AuthenticationPasswordOrAccountIncorrectException(string message = DefaultMessages.AuthenticationPasswordOrAccountIncorrect) : base(message) { }
}
