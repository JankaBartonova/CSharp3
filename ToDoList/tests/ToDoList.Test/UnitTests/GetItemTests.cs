namespace ToDoList.UnitTests;

using Xunit;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class GetItemTests
{
    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
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
        repositoryMock.ReadByIdAsync(item.ToDoItemId).Returns(item);

        // Act
        var result = await controller.ReadById(item.ToDoItemId);

        // Assert
        Assert.IsType<ActionResult<ToDoItemGetResponseDto>>(result);
        repositoryMock.Received(1).ReadByIdAsync(item.ToDoItemId);
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var invalidItemId = 999;
        repositoryMock.ReadByIdAsync(invalidItemId).Returns((ToDoItem)null);

        // Act
        var result = await controller.ReadById(invalidItemId);
        var resultResult = result.Result as ObjectResult;

        // Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).ReadByIdAsync(invalidItemId);
    }

    [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var someItemId = 1;
        repositoryMock.ReadByIdAsync(someItemId).Returns(Task.FromException<ToDoItem>(new Exception("Unhandled exception")));

        // Act
        var result = await controller.ReadById(someItemId);
        var resultResult = result.Result as ObjectResult;

        // Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).ReadByIdAsync(someItemId);
    }
}

