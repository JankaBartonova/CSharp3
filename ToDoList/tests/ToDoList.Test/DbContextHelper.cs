namespace ToDoList.Test;

using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

public class DbContextHelper
{
    public static ToDoItemsContext CreateInMemoryContext()
    {
        var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }
}
