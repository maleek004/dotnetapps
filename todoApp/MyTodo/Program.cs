using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register our AppDbContext with the Dependency Injection container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Your React URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
    
var app = builder.Build();

app.UseCors("ReactAppPolicy");

// List<TodoItem> todos = [];

// 1. GET ALL TASKS
app.MapGet("/todos", async (AppDbContext db) =>
{
    // return Results.Ok(todos);
    var todos = await db.Todos.ToListAsync();
    return Results.Ok(todos);

});

// 2. CREATE A NEW TASK
app.MapPost("/todos", async (TodoItem newTask , AppDbContext db) =>
{
    // Simple logic to automatically generate a unique ID
    // newTask.Id = todos.Count + 1;
    // Add the new item to our C# memory list
    // todos.Add(newTask);
    // Return a 201 Created status code along with the created task
    // return Results.Created($"/todos/{newTask.Id}", newTask);

    // SQL Server handles automatic ID generation, so we don't manually assign Id
    db.Todos.Add(newTask);
    await db.SaveChangesAsync();

    // Return a 201 Created status code along with the created task
    return Results.Created($"/todos/{newTask.Id}", newTask);
});

// 3. UPDATE AN EXISTING TASK
app.MapPut("/todos/{id:int}", async (int id, TodoItem updatedTask , AppDbContext  db) =>
{
    // Search the list for a task with a matching ID
    // var existingTask = todos.FirstOrDefault(t => t.Id == id);
    // If no task is found, return a 404 Not Found error
    // if (existingTask is null)
    // {
        // return Results.NotFound($"Task with ID {id} not found.");
    // }
    // Update the properties of our saved task
    // existingTask.Title = updatedTask.Title;
    // existingTask.IsCompleted = updatedTask.IsCompleted;
    // Return a 204 No Content status code (standard for successful updates)
    // return Results.NoContent();
    // Search the database for a task with a matching ID
    var existingTask = await db.Todos.FindAsync(id);

    // If no task is found, return a 404 Not Found error
    if (existingTask is null)
    {
        // Gentle check: corrected naming alignment to 'todos'
        return Results.NotFound($"Todo with ID {id} not found.");
    }

    // Update the properties of our saved task
    existingTask.Title = updatedTask.Title;
    existingTask.IsCompleted = updatedTask.IsCompleted;

    await db.SaveChangesAsync();

    // Return a 204 No Content status code (standard for successful updates)
    return Results.NoContent();
});

// 4. DELETE A TASK
app.MapDelete("/todos/{id:int}", async (int id, AppDbContext db) =>
{
    // Find the task by ID
    // var existingTask = todos.FirstOrDefault(t => t.Id == id);
    // if (existingTask is null)
    // {
    //     return Results.NotFound($"Task with ID {id} not found.");
    // }
    // Remove the item from our in-memory list
    // todos.Remove(existingTask);
    // return Results.Ok(new { Message = $"Task {id} deleted successfully." });

    // Find the task by ID
    var existingTask = await db.Todos.FindAsync(id);

    if (existingTask is null)
    {
        return Results.NotFound($"Todo with ID {id} not found.");
    }

    // Remove the item from the database DbSet
    db.Todos.Remove(existingTask);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = $"Todo {id} deleted successfully." });
});

app.Run("http://localhost:5000");



public class TodoItem
{
  public  int Id {get; set;} 
  public string Title {get; set;}   = string.Empty;
  public bool IsCompleted {get ; set;}
}
