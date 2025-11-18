namespace ToDoList.Persistence.Repositories;

public interface IRepository<T>
    where T : class
{
    public void Create(T item);

    public bool ExistByName(string name);

    public IEnumerable<T> Read();

    public T? ReadById(int id);

    public void UpdateById(int id, T item);
    public void DeleteById(int id);
}
