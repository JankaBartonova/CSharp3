namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T>
    where T : class
{
    public Task<bool> ExistByNameAsync(string name);

    public Task CreateAsync(T item);

    public Task<IEnumerable<T>> Readsync();

    public Task<T?> ReadByIdAsync(int id);

    public Task UpdateByIdAsync(int id, T item);

    public Task DeleteByIdAsync(int id);
}
