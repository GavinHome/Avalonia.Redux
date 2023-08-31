using samples.Pages.Todos.Page;
using samples.Pages.Counter;
namespace samples;

public static class Routes
{
    public static readonly AbstractRoutes routes = new PageRoutes(
        initialRoute: "todo_list",
        pages: new Dictionary<String, dynamic>
        {
            //// Register TodoList main page
            { "todo_list", new ToDoListPage()},

            //// Register Todo edit page
            //// { "todo_edit", new TodoEditPage()},

            //// Register Count page
            { "count", new CounterPage() },
        });
}