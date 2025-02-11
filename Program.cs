using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// stuff goes here
builder.Services.AddDbContext<ContactDbContext>();

// TODO: Validation
// TODO: Auth
// TODO: Web Security
// TODO: Dont use Domain Models as API Models
app.MapGet("/", async (string query, ContactDbContext db) => await db.Contacts.Where(c => EF.Functions.Like(c.FirstName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.LastName.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Company.Trim(), $"%{query}%") ||
    EF.Functions.Like(c.Email.Trim(), $"%{query}%")
    ).AsNoTracking().ToListAsync());
app.MapGet("/{id:int}", async (ContactDbContext db, int id) => await db.Contacts.FindAsync(id));
app.MapPost("/", async (ContactDbContext db, Contact contact) => await db.Contacts.AddAsync(contact));
app.MapPut("/", async (ContactDbContext db, Contact contact) =>
{
    db.Entry(contact).State = EntityState.Modified;
    await db.SaveChangesAsync();
});


app.Run();
