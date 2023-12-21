namespace samples.Pages.Counter;
using Action = Redux.Action;

public partial class CounterPage
{
    private static Effect<CounterState>? buildEffect() => Redux.Component.EffectConverter.CombineEffects(new Dictionary<object, SubEffect<CounterState>>
    {
        {
            CounterAction.onAdd, _onAdd
        }
    });

    private static async Task _onAdd(Action action, ComponentContext<CounterState> ctx)
    {
        ctx.Dispatch(CounterActionCreator.addAction(1));
        await Task.CompletedTask;
    }
}
