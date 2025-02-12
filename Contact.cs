namespace WebApi;

public class Contact
{
    /// <summary>
    /// Empty/Null = not saved
    /// </summary>
    public Guid? Id { get; set; } = Guid.Empty;
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Company { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
}