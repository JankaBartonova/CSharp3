using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ToDoList.Persistence.DesignTimeDbContextFactory
{
    // Design-time factory so `dotnet ef` can create the correct DbContext for migrations.
    public class ToDoItemsContextFactory : IDesignTimeDbContextFactory<ToDoItemsContext>
    {
        public ToDoItemsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ToDoItemsContext>();

            var connectionString = "DataSource=../../data/localdb.db";

            optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly("ToDoList.Persistence"));
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

            return new ToDoItemsContext(optionsBuilder.Options);
        }
    }
}
