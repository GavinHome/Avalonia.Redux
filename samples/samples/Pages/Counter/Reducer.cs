namespace samples.Pages.Counter;

public partial class CounterPage
{
    private static Reducer<CounterState> buildReducer() => ReducerConverter.asReducers(new Dictionary<object, Reducer<CounterState>>
    {
        {
            CounterAction.add, _add
        }
    });

    private static CounterState _add(CounterState state, Redux.Action action)
    {
        state.Count += action.Payload;
        return state;
    }
}
