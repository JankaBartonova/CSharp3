namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    private readonly ToDoItemsContextBase context;
    public ToDoItemsRepository(ToDoItemsContextBase context)
    {
        this.context = context;
    }

    public bool ExistByName(string name)
    {
        return context.ToDoItems.Any(i => i.Name == name);
    }

    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read()
    {
        return context.ToDoItems.ToList();
    }

    public ToDoItem? ReadById(int id)
    {
        return context.ToDoItems.Where(i => i.ToDoItemId == id).FirstOrDefault();
    }

    public void UpdateById(int id, ToDoItem item)
    {
        var existingItem = context.ToDoItems.Find(id);
        if (existingItem != null)
        {
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.IsCompleted = item.IsCompleted;
            context.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException($"ToDoItem with id {id} not found.");
        }
    }

    public void DeleteById(int id)
    {
        var item = context.ToDoItems.ToList().Find(i => i.ToDoItemId == id);
        if (item != null)
        {
            context.ToDoItems.Remove(item);
            context.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException($"ToDoItem with id {id} not found.");
        }
    }
}
