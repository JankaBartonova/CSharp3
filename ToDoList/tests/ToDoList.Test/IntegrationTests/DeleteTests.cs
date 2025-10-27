namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http.Features;

//using static ToDoList.Test.DbContextMemoryHelper;

public class DeleteTests
{
    [Fact]
    public void Delete_ExistingItem_ShouldReturnNoContent()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item",
            Description = "Item to be deleted",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();
        var controller = new ToDoItemsController(context);

        var items = controller.Read(); // to get ID of item to be deleted
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
        var result = controller.DeleteById(toDoItem.ToDoItemId);
        var getDeleted = controller.ReadById(toDoItem.ToDoItemId);
        //var getItem = getDeleted.Result as NotFoundObjectResult;
        var getItem = getDeleted.Result as ObjectResult;

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(404, getItem.StatusCode);
    }

    [Fact]
    public void Delete_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "DELETE Item 1",
            Description = "Item that does not exist",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        //var controller = new ToDoItemsController(context);

        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();
        var controller = new ToDoItemsController(context);

        // get ID of last item to be sure the tested ID does not exist
        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? itemList.Max(x => x.Id) + 1 : 1;

        // Act
        var result = controller.DeleteById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        context.SaveChanges();
    }
}
