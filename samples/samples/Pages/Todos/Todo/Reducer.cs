namespace samples.Pages.Todos.Todo;

using Action = Redux.Action;

internal partial class TodoComponent
{
    private static Reducer<ToDoState> buildReducer() => ReducerConverter.asReducers(new Dictionary<object, Reducer<ToDoState>>
    {
        {
            ToDoAction.edit, _edit
        },
        {
            ToDoAction.done, _markDone
        }
    });

    private static ToDoState _edit(ToDoState state, Action action)
    {
        ToDoState? toDo = action.Payload;
        if (state.UniqueId == toDo?.UniqueId)
        {
            state.Title = toDo?.Title;
            state.Desc = toDo?.Desc;
        }

        return state;
    }

    private static ToDoState _markDone(ToDoState state, Action action)
    {
        String? uniqueId = action.Payload;
        if (state.UniqueId == uniqueId)
        {
            state.IsDone = !state.IsDone;
        }

        return state;
    }
}
