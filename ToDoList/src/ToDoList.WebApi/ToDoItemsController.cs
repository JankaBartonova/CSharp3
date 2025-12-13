namespace ToDoList.WebApi;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/todoitems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    private readonly IRepositoryAsync<ToDoItem> repository;

    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItemCreateRequestDto request) //localhost:5000/api/todoitems, DTO Data Transfer Object
    {
        ToDoItem item = request.ToDomain();

        if (string.IsNullOrEmpty(item.Name))
        {
            return BadRequest("Name is required");
        }

        if (await repository.ExistByNameAsync(item.Name))
        {
            return Conflict("Item with the same name already exists");
        }

        try
        {
            await repository.CreateAsync(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, item); // 201 + location in header + item in body
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> Read()
    {
        List<ToDoItemGetResponseDto> result = new();

        try
        {
            var dbResult = await repository.ReadAsync();

            result = dbResult.Select(i => ToDoItemGetResponseDto.FromDomain(i)).ToList();
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
    public async Task<ActionResult<ToDoItemGetResponseDto?>> ReadById(int toDoItemId)
    {
        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        try
        {
            ToDoItem? item = await repository.ReadByIdAsync(toDoItemId);
            if (item == null)
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }
            else
            {
                return Ok(new ToDoItemGetResponseDto(item.ToDoItemId, item.Name, item.Description, item.IsCompleted, item.Category));
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
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
            await repository.UpdateByIdAsync(toDoItemId, item);

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
    public async Task<IActionResult> DeleteById(int toDoItemId)
    {
        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        try
        {
            await repository.DeleteByIdAsync(toDoItemId);
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
