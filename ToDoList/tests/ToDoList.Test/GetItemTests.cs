namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetItemTests
{
    [Fact]
    public void Get_ExistingItem_ShouldReturnItem()
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
        var result = controller.ReadById(1);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.Equal("Test Item", value.Name);
        Assert.Equal("Popis", value.Description);
    }

    [Fact]
    public void Get_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.ReadById(999);
        var value = result.GetOther<ProblemDetails, ToDoItemGetResponseDto>();

        // Assert
        Assert.NotNull(value);
        Assert.Equal(404, value.Status);
    }
}

