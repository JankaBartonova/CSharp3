namespace ToDoList.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using Xunit;

public class DeleteTests
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var item = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test Item 1",
            Description = "Description 1",
            IsCompleted = false,
            Category = "AAA"
        };
        repositoryMock.ReadAsync().Returns(new List<ToDoItem> { item });

        // Act
        var result = await controller.DeleteById(item.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).DeleteByIdAsync(item.ToDoItemId);
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var invalidItemId = 999;
        repositoryMock.When(x => x.DeleteByIdAsync(invalidItemId))
                      .Do(x => { throw new KeyNotFoundException(); });

        // Act
        var result = await controller.DeleteById(invalidItemId);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.NotNull(resultResult);
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).DeleteByIdAsync(invalidItemId);
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var itemId = 1;
        repositoryMock.When(x => x.DeleteByIdAsync(itemId))
                      .Do(x => { throw new Exception("Unhandled exception"); });

        // Act
        var result = await controller.DeleteById(itemId);
        var resultResult = result as ObjectResult;

        // Assert
        Assert.NotNull(resultResult);
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).DeleteByIdAsync(itemId);
    }
}
