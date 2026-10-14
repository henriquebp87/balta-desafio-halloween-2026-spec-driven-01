namespace PasswordGenerator.Application.Exceptions;

public sealed class PasswordLengthOutOfRangeException(int minimumLength, int maximumLength)
    : ArgumentOutOfRangeException(
        "length",
        $"O tamanho da senha deve estar entre {minimumLength} e {maximumLength} caracteres.")
{
}
