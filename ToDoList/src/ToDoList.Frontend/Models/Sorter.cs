namespace ToDoList.Frontend.Models;

public interface IToDoItemSorter
{
    public List<ToDoItemView> Order(List<ToDoItemView> items);
}

public class OrderById : IToDoItemSorter
{
    public List<ToDoItemView> Order(List<ToDoItemView> items)
    {
        return items.OrderBy(item => item.Id).ToList();
    }
}

public class OrderByName : IToDoItemSorter
{
    public List<ToDoItemView> Order(List<ToDoItemView> items)
    {
        return items.OrderBy(item => item.Name).ToList();
    }
}
