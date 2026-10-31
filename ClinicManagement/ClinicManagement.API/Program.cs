
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register MVC controllers
builder.Services.AddControllers();

// 2. Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Read SQL Server connection string
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

// 4. Validate connection string
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string is missing. " +
        "Please configure ConnectionStrings:DefaultConnection in appsettings.json."
    );
}

// 5. Register Entity Framework Core and SQL Server
builder.Services.AddDbContext<ClinicDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// 6. Register password hashing service
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// 7. Build application
var app = builder.Build();

// 8. Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 9. Map API controllers
app.MapControllers();

// 10. Run application
app.Run();
