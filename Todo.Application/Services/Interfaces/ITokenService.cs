namespace Todo.Application.Services.Interfaces;

public interface ITokenService
{
    public string Generate(string userName);
}
