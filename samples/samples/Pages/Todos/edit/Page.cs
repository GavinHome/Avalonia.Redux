namespace samples.Pages.Todos.Edit;
using Avalonia.Controls;

public partial class TodoEditPage : Page<TodoEditState, Dictionary<string, dynamic>>
{
    public TodoEditPage() : base(
        initState: initState,
        effect: buildEffect(),
        middlewares: new[]
        {
            Redux.Middlewares.logMiddleware<TodoEditState>(tag: "TodoEditPage")
        },
        view: (state, dispatch, ctx) => new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "TodoEditPage" }
            }
        })
    { }
    
    private static TodoEditState initState(Dictionary<string, dynamic>? param) => new TodoEditState() { toDo = new() };
}
