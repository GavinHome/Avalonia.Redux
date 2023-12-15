using samples.Pages.Todos.Todo;

namespace samples.Pages.Todos.Page;

public class PageState : ReactiveObject
{
    [Reactive] public ObservableCollection<ToDoState>? ToDos { get; init; }
    
    public override string ToString()
    {
        return $"ToDos: {string.Join(String.Empty, ToDos?.Select(x => x.ToString()) ?? Array.Empty<String>())}";
    }
}