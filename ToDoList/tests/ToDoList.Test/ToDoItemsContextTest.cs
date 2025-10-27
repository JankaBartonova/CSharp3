namespace ToDoList.Test;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

public class ToDoItemsContextTest : ToDoItemsContextBase
{
    public ToDoItemsContextTest(string connectionString = "DataSource=../../../IntegrationTests/data/localdb_test.db")
        : base(connectionString)
    {
        this.Database.Migrate();
    }
}
