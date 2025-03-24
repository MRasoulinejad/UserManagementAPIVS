using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<User>();

// GET: All users
app.MapGet("/users", () =>
{
    return Results.Ok(users);
});

// GET: User by ID
app.MapGet("/users/{id:int}", (int id) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Results.Ok(user) : Results.NotFound($"User with ID {id} not found.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }
});

// POST: Add user
app.MapPost("/users", (User user) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(user, null, null);
    if (!Validator.TryValidateObject(user, context, validationResults, true))
    {
        var errors = validationResults.Select(e => e.ErrorMessage);
        return Results.BadRequest(errors);
    }

    if (users.Any(u => u.Email == user.Email))
    {
        return Results.BadRequest("Email already exists.");
    }

    user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// PUT: Update user
app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(updatedUser, null, null);
    if (!Validator.TryValidateObject(updatedUser, context, validationResults, true))
    {
        var errors = validationResults.Select(e => e.ErrorMessage);
        return Results.BadRequest(errors);
    }

    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User with ID {id} not found.");

    if (users.Any(u => u.Email == updatedUser.Email && u.Id != id))
    {
        return Results.BadRequest("Another user with this email already exists.");
    }

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    return Results.Ok(user);
});

// DELETE: Remove user
app.MapDelete("/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound($"User with ID {id} not found.");

    users.Remove(user);
    return Results.Ok($"User with ID {id} deleted.");
});

app.Run();

public record User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}

