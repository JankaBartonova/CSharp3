using ToDoList.Persistence;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContextBase, ToDoItemsContext>();
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

app.Run();
