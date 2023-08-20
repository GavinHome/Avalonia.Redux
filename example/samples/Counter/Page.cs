using Redux;
using Redux.Component;

namespace samples.Counter;

public class Page : Page<CounterState, Dictionary<String, dynamic>>
{
    public Page() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        view: (state, dispatch, ctx) => new Object())
    {}

    private static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 1 };

    private static Effect<CounterState>? buildEffect() => EffectConvert.CombineEffects<CounterState>(null);

    private static Reducer<CounterState> buildReducer() => ReducerConverter.AsReducers<CounterState>(null);
}