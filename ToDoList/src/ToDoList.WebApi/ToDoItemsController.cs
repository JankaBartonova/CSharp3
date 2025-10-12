namespace ToDoList.WebApi;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/todoitems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //localhost:5000/api/todoitems, DTO Data Transfer Object
    {
        ToDoItem item = request.ToDomain();

        if (string.IsNullOrEmpty(item.Name))
        {
            return BadRequest("Name is required");
        }

        if (items.Any(i => i.Name == item.Name))
        {
            return Conflict("Item with the same name already exists");
        }

        try
        {
            item.ToDoItemId = items.Count > 0 ? items.Max(i => i.ToDoItemId) + 1 : 1;
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        string location = Url.Action(nameof(ReadById), "ToDoItems", new { toDoItemId = item.ToDoItemId }, Request.Scheme);
        ToDoItemCreateResponseDto result = new(Url: location, Item: item);

        return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, item); // 201 + location in header + item in body
        //return Created(location, result); // 201 + location in header + location and item in body
        //return CreatedAtAction(nameof(ReadById), "ToDoItems", new { toDoItemId = item.ToDoItemId }, result); // 201 + location in header + location and item in body
    }

    [HttpGet]
    public IActionResult Read()
    {
        List<ToDoItemGetResponseDto> result = new();

        try
        {
            if (items.Count > 0)
            {
                result = items.Select(i => new ToDoItemGetResponseDto(i)).ToList();
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
    public IActionResult ReadById(int toDoItemId)
    {
        //localhost:5000/api/todoitems/1
        //if toDoItemId is not provided, the request will not match this action
        //if toDoItemId is not an integer, the request will not match this action

        if (toDoItemId <= 0)
        {
            return BadRequest("toDoItemId must be greater than zero");
        }

        ToDoItemGetResponseDto result = null;
        try
        {
            if (items.Count == 0)
            {
                return Problem("No ToDos found", null, StatusCodes.Status404NotFound);
            }

            if (items.Find(i => i.ToDoItemId == toDoItemId) == null)
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }

            /*if (!items.Any(i => i.ToDoItemId == toDoItemId))
            {
                return NotFound($"ToDo with id {toDoItemId} not found");
            }*/

            ToDoItem item = items.First(i => i.ToDoItemId == toDoItemId);
            result = new ToDoItemGetResponseDto(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return Ok(result);
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok();
    }
}
