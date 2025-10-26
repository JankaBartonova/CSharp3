namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

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
        var result = controller.ReadById(1);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.Equal("Test Item", value.Name);
        Assert.Equal("Description", value.Description);
    }

    [Fact]
    public void Get_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        using var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var controller = new ToDoItemsController(context);

        // Act
        var result = controller.ReadById(999);
        var value = result.GetOther<ProblemDetails, ToDoItemGetResponseDto>();

        // Assert
        Assert.NotNull(value);
        Assert.Equal(404, value.Status);
    }
}

