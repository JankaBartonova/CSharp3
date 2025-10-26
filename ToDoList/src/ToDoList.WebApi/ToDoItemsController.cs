namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")] //localhost:5000/api/todoitems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    private readonly ToDoItemsContext context;
    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;

        // ToDoItem item = new ToDoItem
        // {
        //     ToDoItemId = 1,
        //     Name = "Prvni ukol",
        //     Description = "Prvni popis",
        //     IsCompleted = false
        // };

        // context.ToDoItems.Add(item);
        // context.SaveChanges();
    }

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //localhost:5000/api/todoitems, DTO Data Transfer Object
    {
        ToDoItem item = request.ToDomain();

        if (string.IsNullOrEmpty(item.Name))
        {
            return BadRequest("Name is required");
        }

        if (context.ToDoItems.Any(i => i.Name == item.Name))
        {
            return Conflict("Item with the same name already exists");
        }

        try
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
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
            if (context.ToDoItems.Count() > 0)
            {
                result = context.ToDoItems.Select(i => new ToDoItemGetResponseDto(i.ToDoItemId, i.Name, i.Description, i.IsCompleted)).ToList();
            }
            else
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
        //localhost:5000/api/todoitems/1

        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        try
        {
            if (context.ToDoItems.Count() == 0)
            {
                return Problem("No ToDos found", null, StatusCodes.Status404NotFound);
            }

            ToDoItem? item = context.ToDoItems.ToList().Find(i => i.ToDoItemId == toDoItemId);
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
            List<ToDoItem> items = context.ToDoItems.ToList();
            int index = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            if (index == -1)
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }

            items[index].Name = item.Name;
            items[index].Description = item.Description;
            items[index].IsCompleted = item.IsCompleted;
            context.SaveChanges();
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
            ToDoItem? existing = context.ToDoItems.ToList().Find(i => i.ToDoItemId == toDoItemId);
            if (existing == null)
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }

            context.ToDoItems.Remove(existing);
            context.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
