using samples.Pages.Todos.Todo;

namespace samples.Pages.Todos.Edit;

public class TodoEditState: ReactiveObject
{
    [Reactive]
    public ToDoState? toDo { get; init; }
}