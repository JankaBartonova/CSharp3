namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

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
            Description = "Popis 1",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Test Item 2",
            Description = "Popis 2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("Test Item 1", firstToDo.Name);
        Assert.Equal("Popis 1", firstToDo.Description);
    }
}
