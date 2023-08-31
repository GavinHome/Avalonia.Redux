using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace samples.Pages.Todos.Todo;

internal class ToDoState: ReactiveObject
{
    [Reactive]
    public string UniqueId { get; init; }

    [Reactive]
    public string? Title { get; set; }

    [Reactive]
    public String? Desc { get; set; }

    [Reactive]
    public bool IsDone { get; set; }

    static decimal _seed = 202308301245;

    public ToDoState(string uniqueId, string title, string desc, bool isDone = false)
    {
        UniqueId = string.IsNullOrEmpty(uniqueId) ? $"{_seed++}" : uniqueId;
        Title = title;
        Desc = desc;
        IsDone = isDone;
    }

    public ToDoState()
    {
        UniqueId = string.IsNullOrEmpty(UniqueId) ? "${_seed++}" : UniqueId;
        IsDone = false;
    }

    public override string ToString()
    {
        return $"ToDoState UniqueId: {UniqueId}, Title: {Title}, Desc: {Desc}, IsDone: {IsDone}";
    }
}
