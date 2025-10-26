namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CreateTests
{
    //private readonly ToDoItemsContext context;
    [Fact]
    public void Create_ValidItem_ShouldReturnCreatedItem()
    {
        // Arrange
        var request = new ToDoItemCreateRequestDto
        (
            Name: "AAA",
            Description: "aaaa",
            IsCompleted: false
        );

        //simulate in memory database
        using var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        /*
        // add values to simulated database
        var entity = new ToDoItem { Name = "AAA", Description = "aaaa", IsCompleted = false };
        context.ToDoItems.Add(entity);
        context.SaveChanges();
        Assert.Equal(1, context.ToDoItems.Count());
        Assert.Equal("AAA", context.ToDoItems.First().Name);*/

        var controller = new ToDoItemsController(context);

        // Act
        var result = controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdItem = Assert.IsType<ToDoItem>(createdResult.Value);
        Assert.Equal(request.Name, createdItem.Name);
        Assert.Equal(request.Description, createdItem.Description);
        Assert.Equal(request.IsCompleted, createdItem.IsCompleted);
        Assert.Equal(201, createdResult.StatusCode);

        // compare result from Create(request) with simulated test database
        //Assert.Equal(createdItem.Name, context.ToDoItems.First().Name);
    }

    [Fact]
    public void Create_ItemWithExistingName_ShouldReturnConflict()
    {
        // Arrange
        var existingItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Existing Item",
            Description = "Existing Description",
            IsCompleted = false
        };

        var context = new ToDoItemsContext("DataSource=:memory:");
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        context.ToDoItems.Add(existingItem);
        context.SaveChanges();

        var controller = new ToDoItemsController(context);
        var request = new ToDoItemCreateRequestDto
        (
            Name: "Existing Item",
            Description: "New Description",
            IsCompleted: true
        );

        // Act
        var result = controller.Create(request);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, conflictResult.StatusCode);
    }
}
