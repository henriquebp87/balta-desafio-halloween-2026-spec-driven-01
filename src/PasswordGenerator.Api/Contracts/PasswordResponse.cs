using PasswordGenerator.Application.Models;

namespace PasswordGenerator.Api.Contracts;

public sealed record PasswordResponse(Guid Guid, string Password)
{
    public static PasswordResponse FromResult(PasswordResult result)
    {
        return new PasswordResponse(result.Guid, result.Password);
    }
}
