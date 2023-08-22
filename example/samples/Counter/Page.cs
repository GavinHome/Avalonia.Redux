using Redux;
using Redux.Component;

namespace samples.Counter;

public class Page : Page<CounterState, Dictionary<String, dynamic>>
{
    public Page() : base(
        initState: CounterState.initState,
        effect: Effect.buildEffect(),
        reducer: Reducer.buildReducer(),
        middlewares: new Middleware<CounterState>[]
        {
            Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterApp")
        },
        view: (state, dispatch, ctx) => new Object())
    { }
}