﻿namespace samples.Pages.Todos.Edit;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Todo;

public class TodoEditState: ReactiveObject
{
    [Reactive]
    public ToDoState? toDo { get; init; }
}