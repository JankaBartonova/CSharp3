namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using static ToDoList.Test.DbContextHelper;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ShouldReturnAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item 1",
            Description = "Description 1",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Test Item 2",
            Description = "Description 2",
            IsCompleted = true
        };

        using var context = CreateInMemoryContext();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        context.SaveChanges();

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("Test Item 1", firstToDo.Name);
        Assert.Equal("Description 1", firstToDo.Description);
    }
}
