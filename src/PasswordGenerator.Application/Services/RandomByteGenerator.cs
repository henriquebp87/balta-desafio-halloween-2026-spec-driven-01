using PasswordGenerator.Application.Abstractions;

namespace PasswordGenerator.Application.Services;

public sealed class RandomByteGenerator : IRandomByteGenerator
{
    public void NextBytes(Span<byte> bytes)
    {
        Random.Shared.NextBytes(bytes);
    }
}
