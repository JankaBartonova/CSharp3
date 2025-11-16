namespace ToDoList.UnitTests;

using Xunit;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.DTOs;

public class GetItemTests
{
    [Fact]
    public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
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
        repositoryMock.ReadById(item.ToDoItemId).Returns(item);

        // Act
        var result = controller.ReadById(item.ToDoItemId);

        // Assert
        Assert.IsType<ActionResult<ToDoItemGetResponseDto>>(result);
        repositoryMock.Received(1).ReadById(item.ToDoItemId);
    }

    [Fact]
    public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var invalidItemId = 999;
        repositoryMock.ReadById(invalidItemId).Returns((ToDoItem)null);

        // Act
        var result = controller.ReadById(invalidItemId);
        var resultResult = result.Result as ObjectResult;

        // Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).ReadById(invalidItemId);
    }

    [Fact]
    public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var someItemId = 1;
        repositoryMock.ReadById(someItemId).Returns(x => throw new Exception("Unhandled exception"));

        // Act
        var result = controller.ReadById(someItemId);
        var resultResult = result.Result as ObjectResult;

        // Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).ReadById(someItemId);
    }
}

