namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItemView
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(250, ErrorMessage = "Description cannot be longer than 250 characters")]
    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    [StringLength(100, ErrorMessage = "Category cannot be longer than 100 characters")]
    public string? Category { get; set; }
}
