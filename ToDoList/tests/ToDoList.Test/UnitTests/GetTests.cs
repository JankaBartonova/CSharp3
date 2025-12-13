namespace ToDoList.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GetTests
{
    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
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

        //Act
        var result = await controller.Read();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        repositoryMock.Received(1).ReadAsync(); //received kolikrat
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);
        repositoryMock.ReadAsync().Returns(new List<ToDoItem>());

        //Act
        var result = await controller.Read();
        var resultResult = result.Result as ObjectResult;

        //Assert
        Assert.Equal(404, resultResult.StatusCode);
        repositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);
        repositoryMock.ReadAsync().Returns(Task.FromException<IEnumerable<ToDoItem>>(new Exception("Unhandled exception")));

        //Act
        var result = await controller.Read();
        var resultResult = result.Result as ObjectResult;

        //Assert
        Assert.Equal(500, resultResult.StatusCode);
        repositoryMock.Received(1).ReadAsync();
    }
}
