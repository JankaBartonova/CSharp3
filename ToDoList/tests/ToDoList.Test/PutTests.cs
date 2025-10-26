namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

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
            Description = "Description",
            IsCompleted = false
        };

        using var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(1, updatedItem);
        var getResult = controller.ReadById(1);
        var getItem = getResult.GetValue<ToDoItemGetResponseDto>();
        var getResult = controller.ReadById(1);
        var getItem = getResult.GetValue<ToDoItemGetResponseDto>();

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Item", getItem.Name);
        Assert.Equal("Updated Description", getItem.Description);
        Assert.True(getItem.IsCompleted);
    }

    [Fact]
    public void Put_NotExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item",
            Description = "Description",
            IsCompleted = false
        };

        using var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Popis", true);

        // Act
        var result = controller.UpdateById(999, updatedItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, (result as ObjectResult)?.StatusCode);
    }
}
