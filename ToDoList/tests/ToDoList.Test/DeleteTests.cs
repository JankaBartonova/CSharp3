namespace ToDoList.Test;

using Xunit;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

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
            Description = "Description",
            IsCompleted = false
        };

        using var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var controller = new ToDoItemsController(context);
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

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

        // Act
        var result = controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
