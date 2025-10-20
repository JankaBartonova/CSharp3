namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;

public class PutTests
{
    [Fact]
    public void Put_ExistingItem_ShouldReturnNoContent()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item",
            Description = "Popis",
            IsCompleted = false
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem);

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Popis", true);

        // Act
        var result = controller.UpdateById(1, updatedItem);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Put_NotExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Popis", true);
        var controller = new ToDoItemsController();

        // Act
        var result = controller.UpdateById(999, updatedItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, (result as ObjectResult)?.StatusCode);
    }
}
