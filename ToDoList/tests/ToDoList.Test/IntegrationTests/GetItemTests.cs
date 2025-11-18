namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;

//using static ToDoList.Test.DbContextMemoryHelper;

public class GetItemTests
{
    [Fact]
    public async void Get_ExistingItem_ShouldReturnItem()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "GET Item",
            Description = "Description",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();

        var items = controller.Read(); // to get ID of item to be retrieved
        var itemList = items.GetValue();
        for (int i = 0; i < itemList.Count(); i++)
        {
            if (itemList.ElementAt(i).Name == "GET Item")
            {
                toDoItem.ToDoItemId = itemList.ElementAt(i).Id;
                break;
            }
        }

        // Act
        var result = controller.ReadById(toDoItem.ToDoItemId);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.Equal("GET Item", value.Name);
        Assert.Equal("Description", value.Description);

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async void Get_NonExistingItem_ShouldReturnNotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "Item 1",
            Description = "Item that does not exist",
            IsCompleted = false
        };

        //using var context = CreateInMemoryContext();
        var context = new ToDoItemsContextTest();
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository: repository);
        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();

        var items = controller.Read();
        var itemList = items.GetValue();
        var nonExistingId = itemList.Any() ? itemList.Max(x => x.Id) + 1 : 1;

        // Act
        var result = controller.ReadById(nonExistingId);
        var value = result.Result as ObjectResult;

        // Assert
        Assert.NotNull(value);
        Assert.Equal(404, value.StatusCode);

        // Clean up
        context.ToDoItems.Remove(toDoItem);
        await context.SaveChangesAsync();
    }
}

