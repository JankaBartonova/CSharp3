namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using static ToDoList.Test.DbContextMemoryHelper;

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

        using var context = CreateInMemoryContext();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(1, updatedItem);
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

        using var context = CreateInMemoryContext();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(999, updatedItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, (result as ObjectResult)?.StatusCode);
    }
}
