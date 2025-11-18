namespace ToDoList.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeleteTests
{
    [Fact]
    public void Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var item = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item 1",
            Description = "Description 1",
            IsCompleted = false
        };
        repositoryMock.Read().Returns(new List<ToDoItem> { item });

        // Act
        var result = controller.DeleteById(item.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).DeleteById(item.ToDoItemId);
    }

    [Fact]
    public void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var invalidItemId = 999;
        repositoryMock.When(x => x.DeleteById(invalidItemId))
                      .Do(x => { throw new KeyNotFoundException(); });

        // Act
        var result = controller.DeleteById(invalidItemId);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).DeleteById(invalidItemId);
    }

    [Fact]
    public void Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var itemId = 1;
        repositoryMock.When(x => x.DeleteById(itemId))
                      .Do(x => { throw new Exception("Unhandled exception"); });

        // Act
        var result = controller.DeleteById(itemId);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).DeleteById(itemId);
    }
}
