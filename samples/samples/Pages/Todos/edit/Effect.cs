namespace samples.Pages.Todos.Edit;
using Action = Redux.Action;

public partial class TodoEditPage
{
    private static Effect<TodoEditState>? buildEffect() => EffectConverter.CombineEffects(new Dictionary<object, SubEffect<TodoEditState>>
    {
        {
            ToDoEditAction.onDone, _onDone
        },
    });

    private static async Task _onDone(Action action, ComponentContext<TodoEditState> ctx)
    {
        await Task.CompletedTask;
    }
}


