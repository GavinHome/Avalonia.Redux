﻿namespace samples.Pages.Todos.Todo;
using Redux.Component;

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
            await Navigator.of(ctx)
            .push<ToDoState>("todo_edit", arguments: ctx.state, (toDo) =>
            {
                if (toDo != null)
                {
                    ctx.Dispatch(ToDoActionCreator.editAction(toDo));
                }
            });
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
