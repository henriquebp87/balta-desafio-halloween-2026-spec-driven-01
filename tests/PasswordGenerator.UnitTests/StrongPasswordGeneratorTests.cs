using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Application.Exceptions;
using PasswordGenerator.Application.Services;

namespace PasswordGenerator.UnitTests;

public sealed class StrongPasswordGeneratorTests
{
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";

    [Theory]
    [InlineData(15)]
    [InlineData(65)]
    public void Generate_WhenLengthIsOutsideAllowedRange_ThrowsValidationException(int length)
    {
        var generator = new StrongPasswordGenerator(new SequentialRandomByteGenerator());

        var exception = Assert.Throws<PasswordLengthOutOfRangeException>(
            () => generator.Generate(length));

        Assert.Contains("entre 16 e 64 caracteres", exception.Message);
    }

    [Theory]
    [InlineData(16)]
    [InlineData(64)]
    public void Generate_WhenLengthIsValid_ReturnsPasswordFollowingRequiredPattern(int length)
    {
        var generator = new StrongPasswordGenerator(new SequentialRandomByteGenerator());

        var password = generator.Generate(length);

        Assert.Equal(length, password.Length);
        Assert.Contains(password, char.IsLetter);
        Assert.Contains(password, char.IsDigit);
        Assert.Contains(password, SpecialCharacters.Contains);
        Assert.DoesNotContain(password, char.IsWhiteSpace);
    }

    private sealed class SequentialRandomByteGenerator : IRandomByteGenerator
    {
        public void NextBytes(Span<byte> bytes)
        {
            for (var index = 0; index < bytes.Length; index++)
            {
                bytes[index] = (byte)index;
            }
        }
    }
}
