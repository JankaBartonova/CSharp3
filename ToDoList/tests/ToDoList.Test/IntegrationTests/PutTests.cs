namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
//using static ToDoList.Test.DbContextMemoryHelper;

public class PutTests
{
    [Fact]
    public async void Put_ExistingItem_ShouldReturnNoContent()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "PUT Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var controller = new ToDoItemsController(context);

        var items = controller.Read(); // to get ID of item to be updated
        var itemList = items.GetValue();
        for (int i = 0; i < itemList.Count(); i++)
        {
            if (itemList.ElementAt(i).Name == "PUT Item")
            {
                toDoItem.ToDoItemId = itemList.ElementAt(i).Id;
                break;
            }
        }

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

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async void Put_NotExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "PUT not existing Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();
        var controller = new ToDoItemsController(context);

        // get ID of last item to be sure the tested ID does not exist
        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? itemList.Max(x => x.Id) + 1 : 1;

        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(nonExistingId, updatedItem);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, (result as ObjectResult)?.StatusCode);

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        await context.SaveChangesAsync();
    }
}
