namespace ToDoList.UnitTests;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using NSubstitute;

//using static ToDoList.Test.DbContextMemoryHelper;

public class PutTests
{
    [Fact]
    public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var itemId = 1;
        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);

        // Act
        var result = controller.UpdateById(itemId, updatedItem);

        // Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).UpdateById(itemId, Arg.Is<ToDoItem>(i =>
            i.Name == updatedItem.Name &&
            i.Description == updatedItem.Description &&
            i.IsCompleted == updatedItem.IsCompleted));
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var invalidItemId = 999;
        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);
        repositoryMock.When(x => x.UpdateById(invalidItemId, Arg.Any<ToDoItem>()))
                      .Do(x => { throw new KeyNotFoundException(); });

        // Act
        var result = controller.UpdateById(invalidItemId, updatedItem);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).UpdateById(invalidItemId, Arg.Is<ToDoItem>(i =>
            i.Name == updatedItem.Name &&
            i.Description == updatedItem.Description &&
            i.IsCompleted == updatedItem.IsCompleted));
    }

    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var itemId = 1;
        var updatedItem = new ToDoItemUpdateRequestDto("Updated Item", "Updated Description", true);
        repositoryMock.When(x => x.UpdateById(itemId, Arg.Any<ToDoItem>()))
                      .Do(x => { throw new Exception("Unhandled exception"); });

        // Act
        var result = controller.UpdateById(itemId, updatedItem);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).UpdateById(itemId, Arg.Is<ToDoItem>(i =>
            i.Name == updatedItem.Name &&
            i.Description == updatedItem.Description &&
            i.IsCompleted == updatedItem.IsCompleted));
    }
}
