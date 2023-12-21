namespace samples.Pages.Counter;
using Action = Redux.Action;

enum CounterAction
{
    add,
    onAdd,
}

internal static class CounterActionCreator
{
    internal static Action addAction(int payload)
    {
        return new Action(CounterAction.add, payload);
    }

    internal static Action onAddAction()
    {
        return new Action(CounterAction.onAdd);
    }
}

