using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using samples.Pages.Todos.Todo;
using System.Collections.ObjectModel;
using System.Linq;

namespace samples.Pages.Todos.Page;

internal class PageState : ReactiveObject
{
    [Reactive]
    public ObservableCollection<ToDoState>? ToDos { get; set; }

    public override string ToString()
    {
        return $"ToDos: {String.Join(String.Empty, ToDos?.Select(x => x.ToString()) ?? Array.Empty<String>())}";
    }
}
