namespace ToDoList.Persistence.Repositories;

public interface IRepository<T>
    where T : class
{
    public bool ExistByName(string name);

    public void Create(T item);

    public IEnumerable<T> Read();

    public T? ReadById(int id);

    public void UpdateById(int id, T item);
    public void DeleteById(int id);
}
