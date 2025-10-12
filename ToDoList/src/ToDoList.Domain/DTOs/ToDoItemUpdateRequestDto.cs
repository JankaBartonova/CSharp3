namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public class ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain() => new() { Name = Name, Description = Description, IsCompleted = IsCompleted };
}
