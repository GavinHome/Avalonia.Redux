﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using samples.Pages.Todos.Todo;
using System.Collections.ObjectModel;
using System.Linq;

namespace samples.Pages.Todos.Page;

public class PageState : ReactiveObject
{
    [Reactive] public ObservableCollection<ToDoState>? ToDos { get; init; }
    
    public override string ToString()
    {
        return $"ToDos: {string.Join(String.Empty, ToDos?.Select(x => x.ToString()) ?? Array.Empty<String>())}";
    }
}