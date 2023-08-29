namespace samples.Pages.Counter;

public partial class CounterPage
{
    private static Effect<CounterState>? buildEffect() => EffectConverter.CombineEffects(new Dictionary<object, SubEffect<CounterState>>
    {
        {
            CounterAction.onAdd, _onAdd
        }
    });

    private static async Task _onAdd(Redux.Action action, ComponentContext<CounterState> ctx)
    {
        ctx.Dispatch(CounterActionCreator.addAction(1));
        ////await Task.Delay(TimeSpan.FromSeconds(5));
        ////ctx.Dispatch(CounterActionCreator.addAction(2));
        await Task.CompletedTask;
    }
}
