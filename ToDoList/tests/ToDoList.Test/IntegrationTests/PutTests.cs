namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using ToDoList.Test.IntegrationTests;

//using static ToDoList.Test.DbContextMemoryHelper;

public class PutTests
{
    [Fact]
    public async void Put_ExistingItem_ShouldReturnNoContent()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "PUT Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(toDoItem.ToDoItemId, updatedItem);
        var getResult = controller.ReadById(toDoItem.ToDoItemId);
        var getItem = getResult.GetValue<ToDoItemGetResponseDto>();

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Item", getItem.Name);
        Assert.Equal("Updated Description", getItem.Description);
        Assert.True(getItem.IsCompleted);

        CleanUp.CleanUpAfterTest(context);
    }

    [Fact]
    public async void Put_NotExistingItem_ShouldReturnNotFound()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "PUT not existing Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        // get ID of last item to be sure the tested ID does not exist
        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? Int32.MaxValue : 1;

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(nonExistingId, updatedItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, (result as ObjectResult)?.StatusCode);

        CleanUp.CleanUpAfterTest(context);
    }
}
