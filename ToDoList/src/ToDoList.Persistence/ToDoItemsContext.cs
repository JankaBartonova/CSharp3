namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContextBase : DbContext
{
    private readonly string connectionString;
    public ToDoItemsContextBase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}

public class ToDoItemsContext : ToDoItemsContextBase
{
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db")
        : base(connectionString)
    {
        this.Database.Migrate();
    }
}
