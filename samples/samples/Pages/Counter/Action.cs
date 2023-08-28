namespace samples.Counter;
using Action = Redux.Action;

enum CounterAction
{
    add,
    //minus,
    onAdd,
    //onMinus,
}

internal class CounterActionCreator
{
    internal static Action addAction(int payload)
    {
        return new Action(CounterAction.add, payload);
    }

    //internal static Action minusAction(int payload)
    //{
    //    return new Action(CounterAction.minus, payload);
    //}

    internal static Action onAddAction()
    {
        return new Action(CounterAction.onAdd);
    }

    //internal static Action onMinusAction()
    //{
    //    return new Action(CounterAction.onAdd);
    //}
}

