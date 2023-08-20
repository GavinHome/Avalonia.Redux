using System.Runtime.InteropServices.JavaScript;
using Redux;
using Redux.Component;

namespace samples;

public class CounterPage : Page<CounterState, Dictionary<String, dynamic>>
{
    public CounterPage() : base(
        initState: param => new CounterState(),
        effect: BuildEffect(),
        reducer: BuildReducer(),
        view: (CounterState state, Dispatch dispatch, ComponentContext<CounterState> ctx) =>
        {
            return new Object();
        })
    {}
    
    private static Effect<CounterState>? BuildEffect() => EffectConvert.CombineEffects<CounterState>(null);

    private static Reducer<CounterState> BuildReducer() => ReducerConverter.AsReducers<CounterState>(null);
}

public class CounterState
{
   public int Count => 0;
}