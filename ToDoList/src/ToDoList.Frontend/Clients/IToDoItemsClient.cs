namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;

using ToDoList.Frontend.Models;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();

    public Task<ToDoItemView?> ReadItemByIdAsync(int ItemId);

    public Task UpdateItemAsync(ToDoItemView updatedItem);

    public Task DeleteItemAsync(int ItemId);
}
