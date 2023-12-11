namespace samples.Pages.Todos.Edit;
using Action = Redux.Action;

enum ToDoEditAction { onDone, onChangeTheme }

internal static class ToDoEditActionCreator
{
    internal static Action onDone()
    {
        return new Action(ToDoEditAction.onDone);
    }

    internal static Action onChangeTheme()
    {
        return new Action(ToDoEditAction.onChangeTheme);
    }
}