namespace ToDoList.Domain.DTOs;

public record ToDoItemCreateRequestDto(string Title, string Description, bool IsCompleted)
{

}
