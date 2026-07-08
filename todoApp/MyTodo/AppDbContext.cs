using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // The constructor forwards configuration settings (like connection strings) to the base DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // This property registers a database table. 
    // EF Core will look at your 'TodoItem' class and automatically map it to a table named 'Todos'
    public DbSet<TodoItem> Todos { get; set; }
}