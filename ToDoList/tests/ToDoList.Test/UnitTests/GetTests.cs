namespace ToDoList.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
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

        //Act
        var result = controller.Read();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        repositoryMock.Received(1).Read(); //received kolikrat
    }

    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);
        repositoryMock.Read().Returns(new List<ToDoItem>());

        //Act
        var result = controller.Read();
        var resultResult = result.Result as ObjectResult;

        //Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).Read();
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);
        repositoryMock.Read().Returns(x => throw new Exception("Unhandled exception"));

        //Act
        var result = controller.Read();
        var resultResult = result.Result as ObjectResult;

        //Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).Read();
    }
}
