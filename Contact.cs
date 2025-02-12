namespace WebApi;

public class Contact
{
    /// <summary>
    /// Empty = not saved
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Company { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}

public class ContactModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; }= "";
    public string Company { get; set; }= "";
    public string Email { get; set; }= "";
    public string Phone { get; set; }= "";
}