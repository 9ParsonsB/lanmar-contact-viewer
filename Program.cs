using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLamar((_, registry) => registry.Scan(scan =>
{
    scan.AssemblyContainingType<Program>();
    scan.WithDefaultConventions();
    scan.LookForRegistries();
}));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContactDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// TODO: Validation
// TODO: Auth
// TODO: Web Security
// TODO: Dont use Domain Models as API Models
app.MapGet("/", async ([FromServices] ContactDbContext db, string query = "") => await db.Contacts.Where(c => EF.Functions.Like(c.FirstName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.LastName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Company.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Email.Trim(), $"%{query}%")
    ).AsNoTracking().ToListAsync());
app.MapGet("/{id:guid}", async ([FromServices] ContactDbContext db, Guid id) => await db.Contacts.FindAsync(id));
app.MapPost("/", async ([FromServices] ContactDbContext db, Contact contact) =>
{
    contact.Id = Guid.NewGuid();    
    await db.Contacts.AddAsync(contact);
    await db.SaveChangesAsync();
    return contact.Id;
});
app.MapPut("/", async ([FromServices] ContactDbContext db, Contact contact) =>
{
    db.Entry(contact).State = EntityState.Modified;
    await db.SaveChangesAsync();
});

await app.Services.GetRequiredService<ContactDbContext>().Database.EnsureCreatedAsync();

await app.RunAsync();
