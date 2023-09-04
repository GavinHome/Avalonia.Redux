using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using samples.Pages.Todos.Todo;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;

namespace samples.Pages.Todos.Page;

public class PageState : ReactiveObject
{
    [Reactive] public ObservableCollection<ToDoState>? ToDos { get; set; }
    // public Task<Control> Report { get; set; }
    // public Task<List<Control>> TodoList { get; set; }
    
    public override string ToString()
    {
        return $"ToDos: {String.Join(String.Empty, ToDos?.Select(x => x.ToString()) ?? Array.Empty<String>())}";
    }
}