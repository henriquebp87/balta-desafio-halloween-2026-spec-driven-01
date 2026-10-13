namespace PasswordGenerator.Domain.Entities;

public sealed class Password
{
    public Guid Id { get; set; }

    public Guid Guid { get; set; }

    public string Value { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
