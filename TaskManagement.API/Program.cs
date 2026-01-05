using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Extensions;
using TaskManagement.API.Middleware;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Middleware

// Infrastructure middleware
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed user accounts

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = new PasswordHasher<User>();

    var usersToSeed = new List<(string Username, string Password)>
    {
        ("admin", "admin123"),
        ("user1", "user123"),
        ("user2", "user123")
    };

    foreach (var (username, password) in usersToSeed)
    {
        if (!context.Users.Any(u => u.Username == username))
        {
            var user = new User(username);
            user.SetPasswordHash(hasher.HashPassword(user, password));

            context.Users.Add(user);
        }
    }

    context.SaveChanges();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<BasicAuthMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
