namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/todoitems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    private readonly IRepository<ToDoItem> repository;

    public ToDoItemsController(IRepository<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //localhost:5000/api/todoitems, DTO Data Transfer Object
    {
        ToDoItem item = request.ToDomain();

        if (string.IsNullOrEmpty(item.Name))
        {
            return BadRequest("Name is required");
        }

        if (repository.ExistByName(item.Name))
        {
            return Conflict("Item with the same name already exists");
        }

        try
        {
            repository.Create(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, item); // 201 + location in header + item in body
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        List<ToDoItemGetResponseDto> result = new();

        try
        {
            result = repository.Read().Select(i => ToDoItemGetResponseDto.FromDomain(i)).ToList();
            if (result.Count == 0)
            {
                return Problem("No ToDos found", null, StatusCodes.Status404NotFound);
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return Ok(result);
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto?> ReadById(int toDoItemId)
    {
        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        try
        {
            /*if (repository.ToDoItems.Count() == 0)
            {
                return Problem("No ToDos found", null, StatusCodes.Status404NotFound);
            }*/

            ToDoItem? item = repository.ReadById(toDoItemId);
            if (item == null)
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }
            else
            {
                return Ok(new ToDoItemGetResponseDto(item.ToDoItemId, item.Name, item.Description, item.IsCompleted));
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {

        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        if (request == null)
        {
            return BadRequest("Request body is required");
        }

        ToDoItem item = request.ToDomain();

        if (string.IsNullOrEmpty(item.Name))
        {
            return BadRequest("Name is required");
        }

        if (string.IsNullOrEmpty(item.Description))
        {
            return BadRequest("Name is required");
        }

        try
        {
            repository.UpdateById(toDoItemId, item);

        }
        catch (KeyNotFoundException)
        {
            return NotFound($"ToDo with id {toDoItemId} not found");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        try
        {
            repository.DeleteById(toDoItemId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"ToDo with id {toDoItemId} not found");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
