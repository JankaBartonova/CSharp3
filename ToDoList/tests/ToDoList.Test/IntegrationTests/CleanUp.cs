namespace ToDoList.Test.IntegrationTests;

public class CleanUp
{
    public static async void CleanUpBeforeTest(ToDoItemsContextTest context)
    {
        if (context.ToDoItems.Any())
        {
            context.ToDoItems.RemoveRange(context.ToDoItems);
            await context.SaveChangesAsync();
        }
    }

    public static async void CleanUpAfterTest(ToDoItemsContextTest context)
    {
        if (context.ToDoItems.Any())
        {
            context.ToDoItems.RemoveRange(context.ToDoItems);
            await context.SaveChangesAsync();
        }
    }
}
