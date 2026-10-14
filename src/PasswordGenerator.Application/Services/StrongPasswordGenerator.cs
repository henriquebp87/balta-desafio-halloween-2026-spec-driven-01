using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Application.Exceptions;

namespace PasswordGenerator.Application.Services;

public sealed class StrongPasswordGenerator(IRandomByteGenerator randomByteGenerator) : IPasswordGenerator
{
    public const int MinimumLength = 16;
    public const int MaximumLength = 64;

    private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";
    private const string AllCharacters = Letters + Numbers + SpecialCharacters;

    public string Generate(int length)
    {
        if (length is < MinimumLength or > MaximumLength)
        {
            throw new PasswordLengthOutOfRangeException(MinimumLength, MaximumLength);
        }

        Span<byte> randomBytes = stackalloc byte[length * 2];
        randomByteGenerator.NextBytes(randomBytes);

        Span<char> password = stackalloc char[length];
        password[0] = PickCharacter(Letters, randomBytes[0]);
        password[1] = PickCharacter(Numbers, randomBytes[1]);
        password[2] = PickCharacter(SpecialCharacters, randomBytes[2]);

        for (var index = 3; index < password.Length; index++)
        {
            password[index] = PickCharacter(AllCharacters, randomBytes[index]);
        }

        for (var index = password.Length - 1; index > 0; index--)
        {
            var targetIndex = randomBytes[length + index] % (index + 1);
            (password[index], password[targetIndex]) = (password[targetIndex], password[index]);
        }

        return new string(password);
    }

    private static char PickCharacter(string characterSet, byte randomByte)
    {
        return characterSet[randomByte % characterSet.Length];
    }
}
