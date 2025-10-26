namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using static ToDoList.Test.DbContextHelper;

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
        using var context = CreateInMemoryContext();

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

        using var context = CreateInMemoryContext();
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
