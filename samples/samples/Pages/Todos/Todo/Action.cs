namespace samples.Pages.Todos.Todo;
using Action = Redux.Action;

enum ToDoAction { onEdit, edit, done, onRemove, remove }

internal static class ToDoActionCreator
{
    internal static Action onEditAction(String uniqueId)
    {
        return new Action(ToDoAction.onEdit, payload: uniqueId);
    }

    internal static Action editAction(ToDoState toDo)
    {
        return new Action(ToDoAction.edit, payload: toDo);
    }

    internal static Action doneAction(String uniqueId)
    {
        return new Action(ToDoAction.done, payload: uniqueId);
    }

    internal static Action onRemoveAction(String uniqueId)
    {
        return new Action(ToDoAction.onRemove, payload: uniqueId);
    }

    internal static Action removeAction(String uniqueId)
    {
        return new Action(ToDoAction.remove, payload: uniqueId);
    }
}

