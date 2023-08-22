using Redux;
using Redux.Component;

namespace samples.Counter;

internal class Effect
{
    internal static Effect<CounterState>? buildEffect() => EffectConverter.CombineEffects<CounterState>(new Dictionary<object, SubEffect<CounterState>>()
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
