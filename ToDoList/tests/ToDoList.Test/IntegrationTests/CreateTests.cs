namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
//using static ToDoList.Test.DbContextMemoryHelper;

public class CreateTests
{
    [Fact]
    public async void Create_ValidItem_ShouldReturnCreatedItem()
    {
        // Arrange
        var request = new ToDoItemCreateRequestDto
        (
            Name: "POST Item",
            Description: "Description",
            IsCompleted: false
        );

        //simulate in memory database
        //using var context = CreateInMemoryContext();

        // use production code with TEST database
        var context = new ToDoItemsContextTest();
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

        // Clean up
        context.ToDoItems.Remove(createdItem);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async void Create_ItemWithExistingName_ShouldReturnConflict()
    {
        // Arrange
        var existingItem = new ToDoItem
        {
            Name = "POST Existing Item",
            Description = "Existing Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();

        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(existingItem);
        await context.SaveChangesAsync();

        var controller = new ToDoItemsController(context);
        var request = new ToDoItemCreateRequestDto
        (
            Name: "POST Existing Item",
            Description: "New Description",
            IsCompleted: true
        );

        // Act
        var result = controller.Create(request);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, conflictResult.StatusCode);

        // Clean up
        context.ToDoItems.Remove(existingItem);
        await context.SaveChangesAsync();
    }
}
