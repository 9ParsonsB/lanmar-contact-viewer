using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class ContactDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    
    public string DbPath { get; }
    
    public ContactDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "contacts.db");
    }
        
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}