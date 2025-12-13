namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContextBase context;
    public ToDoItemsRepository(ToDoItemsContextBase context)
    {
        this.context = context;
    }

    public async Task<bool> ExistByNameAsync(string name)
    {
        return await context.ToDoItems.AnyAsync(i => i.Name == name);
    }

    public async Task CreateAsync(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAsync()
    {
        return await context.ToDoItems.ToListAsync();
    }

    public async Task<ToDoItem?> ReadByIdAsync(int id)
    {
        return await context.ToDoItems.Where(i => i.ToDoItemId == id).FirstOrDefaultAsync();
    }

    public async Task UpdateByIdAsync(int id, ToDoItem item)
    {
        var existingItem = await context.ToDoItems.FindAsync(id);
        if (existingItem != null)
        {
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.IsCompleted = item.IsCompleted;
            existingItem.Category = item.Category;
            await context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"ToDoItem with id {id} not found.");
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        var item = await context.ToDoItems.FindAsync(id);
        if (item != null)
        {
            context.ToDoItems.Remove(item);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"ToDoItem with id {id} not found.");
        }
    }
}
