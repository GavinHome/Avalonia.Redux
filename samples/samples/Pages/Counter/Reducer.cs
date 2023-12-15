namespace samples.Pages.Counter;
using Action = Redux.Action;

public partial class CounterPage
{
    private static Reducer<CounterState> buildReducer() => ReducerConverter.AsReducers(new Dictionary<object, Reducer<CounterState>>
    {
        {
            CounterAction.add, _add
        }
    });

    private static CounterState _add(CounterState state, Action action)
    {
        state.Count += action.Payload;
        return state;
    }
}
