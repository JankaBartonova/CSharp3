namespace ToDoList.UnitTests;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using System.Linq;
using System.Collections.Generic;

public class CreateTests
{
    [Fact]
    public void Create_ValidItem_ShouldReturnCreatedItem()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository: repositoryMock);

        var request = new ToDoItemCreateRequestDto
        (
            Name: "POST Item",
            Description: "Description",
            IsCompleted: false
        );

        // Act
        var result = controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdItem = Assert.IsType<ToDoItem>(createdResult.Value);
        Assert.Equal(request.Name, createdItem.Name);
        Assert.Equal(request.Description, createdItem.Description);
        Assert.Equal(request.IsCompleted, createdItem.IsCompleted);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public void Create_ItemWithExistingName_ShouldReturnConflict()
    {
        // Arrange
        var existingItem = new ToDoItem
        {
            Name = "POST Existing Item",
            Description = "Existing Description",
            IsCompleted = false
        };

        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        repositoryMock.ExistByName(existingItem.Name).Returns(true);
        var controller = new ToDoItemsController(repository: repositoryMock);

        var request = new ToDoItemCreateRequestDto
        (
            Name: "POST Existing Item",
            Description: "New Description",
            IsCompleted: true
        );

        // Act
        var result = controller.Create(request);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, conflictResult.StatusCode);
    }
}
