namespace Gymly.Application.Interfaces.Common;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}