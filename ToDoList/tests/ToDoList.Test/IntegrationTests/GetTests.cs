namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence.Repositories;

//using static ToDoList.Test.DbContextMemoryHelper;

public class GetTests
{
    [Fact]
    public async Task Get_AllItems_ShouldReturnAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Test Item 1",
            Description = "Description 1",
            IsCompleted = false,
            Category = "AAA"
        };

        var toDoItem2 = new ToDoItem
        {
            Name = "Test Item 2",
            Description = "Description 2",
            IsCompleted = true,
            Category = "AAA"
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        await context.SaveChangesAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        // Act
        var result = await controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal("Test Item 1", firstToDo.Name);
        Assert.Equal("Description 1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);
        Assert.Equal("AAA", firstToDo.Category);

        var secondToDo = value.Skip(1).First();
        Assert.Equal("Test Item 2", secondToDo.Name);
        Assert.Equal("Description 2", secondToDo.Description);
        Assert.True(secondToDo.IsCompleted);
        Assert.Equal("AAA", secondToDo.Category);

        // Clean up
        context.ToDoItems.Remove(toDoItem1);
        context.ToDoItems.Remove(toDoItem2);
        await context.SaveChangesAsync();
    }
}
