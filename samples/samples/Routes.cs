using samples.Pages.Counter;
using samples.Pages.Todos.Page;
using samples.Pages.Todos.Edit;

namespace samples;

public static class Routes
{
    public static readonly AbstractRoutes routes = new PageRoutes(
        initialRoute: "todo_list",
        pages: new Dictionary<String, dynamic>
        {
            //// Register TodoList page
            { "todo_list", new ToDoListPage()},

            //// Register TodoEdit page
            { "todo_edit", new TodoEditPage()},

            //// Register Counter page
            { "count", new CounterPage() },
        });
}