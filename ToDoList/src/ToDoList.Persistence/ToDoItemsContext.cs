namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContextBase : DbContext
{
    private readonly string connectionString;

    //for migrations
    // private readonly string? connectionString;

    public ToDoItemsContextBase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    /*//for migrations: Design-time / DI-friendly constructor that accepts configured DbContextOptions.
    public ToDoItemsContextBase(DbContextOptions options)
        : base(options)
    {
        // connectionString remains null when options are used.
    }*/

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);

        /*//for migrations: Only configure the provider here if options have not been configured yet.
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseSqlite(connectionString);
        }*/
    }
}

public class ToDoItemsContext : ToDoItemsContextBase
{
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db")
        : base(connectionString)
    {
        this.Database.Migrate();
    }

    /*//for migrations: Add this constructor so EF/design-time code can pass configured DbContextOptions.
    public ToDoItemsContext(DbContextOptions<ToDoItemsContext> options)
        : base(options)
    {
    }*/
}
