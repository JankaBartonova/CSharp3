namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using ToDoList.Test.IntegrationTests;

//using Microsoft.AspNetCore.Http.Features;

//using static ToDoList.Test.DbContextMemoryHelper;

public class DeleteTests
{
    [Fact]
    public async void Delete_ExistingItem_ShouldReturnNoContent()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item",
            Description = "Item to be deleted",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);
        var getDeleted = controller.ReadById(toDoItem.ToDoItemId);
        //var getItem = getDeleted.Result as NotFoundObjectResult;
        var getItem = getDeleted.Result as ObjectResult;

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(404, getItem.StatusCode);

        CleanUp.CleanUpAfterTest(context);
    }

    [Fact]
    public async void Delete_NonExistingItem_ShouldReturnNotFound()
    {
        var context = new ToDoItemsContextTest();
        CleanUp.CleanUpBeforeTest(context);

        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item 1",
            Description = "Item that does not exist",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        // get ID of last item to be sure the tested ID does not exist
        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? Int32.MaxValue : 1;

        // Act
        var result = controller.DeleteById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);

        CleanUp.CleanUpAfterTest(context);
    }
}
