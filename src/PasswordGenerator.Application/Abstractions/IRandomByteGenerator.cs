namespace PasswordGenerator.Application.Abstractions;

public interface IRandomByteGenerator
{
    void NextBytes(Span<byte> bytes);
}
