namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
//using static ToDoList.Test.DbContextMemoryHelper;

public class GetTests
{
    [Fact]
    public async void Get_AllItems_ShouldReturnAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Test Item 1",
            Description = "Description 1",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            Name = "Test Item 2",
            Description = "Description 2",
            IsCompleted = true
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        await context.SaveChangesAsync();

        var controller = new ToDoItemsController(context);

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal("Test Item 1", firstToDo.Name);
        Assert.Equal("Description 1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);

        var secondToDo = value.Skip(1).First();
        Assert.Equal("Test Item 2", secondToDo.Name);
        Assert.Equal("Description 2", secondToDo.Description);
        Assert.True(secondToDo.IsCompleted);

        // Clean up
        context.ToDoItems.Remove(toDoItem1);
        context.ToDoItems.Remove(toDoItem2);
        await context.SaveChangesAsync();
    }
}
