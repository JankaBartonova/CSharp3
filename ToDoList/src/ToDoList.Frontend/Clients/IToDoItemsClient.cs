using ToDoList.Frontend.Models;

namespace ToDoList.Frontend.Clients;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();

    public Task<ToDoItemView?> ReadItemByIdAsync(int ItemId);

    public Task UpdateItemAsync(ToDoItemView updatedItem);
}
