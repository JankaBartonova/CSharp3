namespace ToDoList.Test;

using Xunit;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class DeleteTests
{
    [Fact]
    public void Delete_ExistingItem_ShouldReturnNoContent()
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

        // Act
        var result = controller.DeleteById(1);
        var getResult = controller.ReadById(1);
        var getItem = getResult.GetOther<ProblemDetails, ToDoItemGetResponseDto>();

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(404, getItem.Status);
    }

    [Fact]
    public void Delete_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
