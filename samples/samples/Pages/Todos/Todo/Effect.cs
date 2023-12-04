namespace samples.Pages.Todos.Todo;
using Action = Redux.Action;

internal partial class TodoComponent
{
    private static Effect<ToDoState>? buildEffect() => EffectConverter.CombineEffects(new Dictionary<object, SubEffect<ToDoState>>
    {
        {
            ToDoAction.onEdit, _onEdit
        },
        {
            ToDoAction.onRemove, _onRemove
        }
    });

    private static async Task _onEdit(Redux.Action action, ComponentContext<ToDoState> ctx)
    {
        if (action.Payload == ctx.state.UniqueId)
        {
            Navigator.of(ctx)
            .pushNamed("todo_edit", arguments: ctx.state)
            .then((toDo) =>
            {
                if (toDo != null)
                {
                    //ctx.Dispatch(ToDoActionCreator.editAction(toDo));
                }
            });

            ////ctx.Dispatch(ToDoActionCreator.editAction(new ToDoState()
            ////{
            ////    UniqueId = ctx.state.UniqueId,
            ////    Title = ctx.state.Title + new Random().Next(65535),
            ////    Desc = ctx.state.Desc + new Random().Next(65535),
            ////}));
        }

        await Task.CompletedTask;
    }

    private static async Task _onRemove(Redux.Action action, ComponentContext<ToDoState> ctx)
    {
        if (action.Payload == ctx.state.UniqueId)
        {
            ctx.Dispatch(ToDoActionCreator.removeAction(ctx.state.UniqueId));
        }

        await Task.CompletedTask;
    }
}
