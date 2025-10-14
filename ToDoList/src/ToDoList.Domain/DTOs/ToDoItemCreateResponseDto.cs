using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

public record ToDoItemCreateResponseDto(string Url, ToDoItem Item)
{
}
