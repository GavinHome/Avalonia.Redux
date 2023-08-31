using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using samples.Pages.Todos.Todo;
using System.Collections.ObjectModel;
using System.Linq;

namespace samples.Pages.Todos.Page;

public class PageState : ReactiveObject
{
    [Reactive]
    public List<ToDoState>? ToDos { get; set; }

    public override string ToString()
    {
        return $"ToDos: {String.Join(String.Empty, ToDos?.Select(x => x.ToString()) ?? Array.Empty<String>())}";
    }
}
