namespace ToDoList.Test;

using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ShouldReturnAllItems()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item",
            Description = "Popis",
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
        controller.AddItemToStorage(toDoItem);
        controller.AddItemToStorage(toDoItem2);

        // Act
        var result = controller.Read();
        var value = result;
    }
}
