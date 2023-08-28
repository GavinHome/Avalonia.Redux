namespace samples.Counter;

public partial class CounterPage
{
    internal static Effect<CounterState>? buildEffect() => EffectConverter.CombineEffects(new Dictionary<object, SubEffect<CounterState>>
    {
        {
            CounterAction.onAdd, _onAdd
        }
    });

    private static async Task _onAdd(Redux.Action action, ComponentContext<CounterState> ctx)
    {
        await ctx.Dispatch(CounterActionCreator.addAction(1));
    }
}
