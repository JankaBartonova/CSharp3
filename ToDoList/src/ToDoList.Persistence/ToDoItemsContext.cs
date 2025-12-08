namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContextBase : DbContext
{
    private readonly string? connectionString;

    // Runtime constructor that accepts a connection string.
    public ToDoItemsContextBase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // Design-time / DI-friendly constructor that accepts configured DbContextOptions.
    public ToDoItemsContextBase(DbContextOptions options)
        : base(options)
    {
        // connectionString remains null when options are used.
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure the provider here if options have not been configured yet.
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}

public class ToDoItemsContext : ToDoItemsContextBase
{
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db")
        : base(connectionString)
    {
        this.Database.Migrate();
    }

    // Add this constructor so EF/design-time code can pass configured DbContextOptions.
    public ToDoItemsContext(DbContextOptions<ToDoItemsContext> options)
        : base(options)
    {
    }
}
