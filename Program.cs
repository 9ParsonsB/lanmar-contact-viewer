using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
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
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicy()
        { Origins = { "http://localhost:7231" }, Headers = { "*" }, Methods = { "*" } });
});

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

app.MapGet("/api", async ([FromServices] ContactDbContext db, string query = "") => await db.Contacts.Where(c => EF.Functions.Like(c.FirstName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.LastName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Company.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Email.Trim(), $"%{query}%")
    ).AsNoTracking().ToListAsync());

app.MapGet("/api/{id:guid}", async ([FromServices] ContactDbContext db, Guid id) =>
{
    var result = await db.Contacts.FindAsync(id); 
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapPost("/api", async ([FromServices] ContactDbContext db, [FromBody] ContactModel c) =>
{
    var contact = new Contact()
    {
        Id = Guid.NewGuid(),  
        FirstName = c.FirstName,
        LastName = c.LastName,
        Company = c.Company,
        Email = c.Email,
        Phone = c.Phone
    };
    await db.Contacts.AddAsync(contact);
    await db.SaveChangesAsync();
    return Results.Created();
});

app.MapPut("/api", async ([FromServices] ContactDbContext db, [FromBody] Contact contact) =>
{
    db.Entry(contact).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.Accepted();
});

app.MapReverseProxy();


await app.Services.GetRequiredService<ContactDbContext>().Database.EnsureCreatedAsync();

await app.RunAsync();
