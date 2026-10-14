namespace PasswordGenerator.Application.Services;

public interface IPasswordGenerator
{
    string Generate(int length);
}
