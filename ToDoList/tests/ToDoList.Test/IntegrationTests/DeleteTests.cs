namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;

//using Microsoft.AspNetCore.Http.Features;

//using static ToDoList.Test.DbContextMemoryHelper;

public class DeleteTests
{
    [Fact]
    public async Task Delete_ExistingItem_ShouldReturnNoContent()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item",
            Description = "Item to be deleted",
            IsCompleted = false,
            Category = "AAA"
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        var items = await controller.Read(); // to get ID of item to be deleted
        var itemList = items.GetValue();
        for (int i = 0; i < itemList.Count(); i++)
        {
            if (itemList.ElementAt(i).Name == "DELETE Item")
            {
                toDoItem.ToDoItemId = itemList.ElementAt(i).Id;
                break;
            }
        }

        // Act
        var result = await controller.DeleteById(toDoItem.ToDoItemId);
        var getDeleted = await controller.ReadById(toDoItem.ToDoItemId);
        //var getItem = getDeleted.Result as NotFoundObjectResult;
        var getItem = getDeleted.Result as ObjectResult;

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(404, getItem.StatusCode);

        // Clean up
        if (context.ToDoItems.Any(t => t.ToDoItemId == toDoItem.ToDoItemId))
        {
            context.ToDoItems.Remove(toDoItem);
            await context.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task Delete_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item 1",
            Description = "Item that does not exist",
            IsCompleted = false,
            Category = "AAA"
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);

        // get ID of last item to be sure the tested ID does not exist
        var items = await controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? Int32.MaxValue : 1;

        // Act
        var result = await controller.DeleteById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        await context.SaveChangesAsync();
    }
}
