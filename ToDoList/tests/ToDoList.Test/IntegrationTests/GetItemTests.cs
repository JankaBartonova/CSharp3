namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using ToDoList.Test.IntegrationTests;

//using static ToDoList.Test.DbContextMemoryHelper;

public class GetItemTests
{
    [Fact]
    public async void Get_ExistingItem_ShouldReturnItem()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "GET Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();

        // Act
        var result = controller.ReadById(toDoItem.ToDoItemId);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.Equal("GET Item", value.Name);
        Assert.Equal("Description", value.Description);

        CleanUp.CleanUpAfterTest(context);
    }

    [Fact]
    public async void Get_NonExistingItem_ShouldReturnNotFound()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "Item 1",
            Description = "Item that does not exist",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();

        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? Int32.MaxValue : 1;

        // Act
        var result = controller.ReadById(nonExistingId);
        var value = result.Result as ObjectResult;

        // Assert
        Assert.NotNull(value);
        Assert.Equal(404, value.StatusCode);

        CleanUp.CleanUpAfterTest(context);
    }
}

