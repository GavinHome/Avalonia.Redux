using samples.Pages.Todos.Todo;

namespace samples.Pages.Todos.Page;
using Action = Redux.Action;

public partial class ToDoListPage
{
    private static Reducer<PageState> buildReducer() => ReducerConverter.AsReducers(new Dictionary<object, Reducer<PageState>>
    {
        { "initToDos", _init },
        { "add", _add },
        { ToDoAction.remove, _remove }
    });

    private static PageState _init(PageState state, Action action)
    {
        List<ToDoState> toDos = action.Payload ?? new List<ToDoState>();
        state.ToDos?.AddRange(toDos);
        return state;
    }

    private static PageState _add(PageState state, Action action)
    {
        ToDoState? toDo = action.Payload;
        if (toDo != null)
        {
            state.ToDos!.Add(toDo);
        }

        return state;
    }

    private static PageState _remove(PageState state, Action action)
    {
        string? uniqueId = action.Payload;
        var item = state.ToDos?.SingleOrDefault(todo => todo.UniqueId == uniqueId);
        if (item != null) state.ToDos?.Remove(item);
        return state;
    }
}
