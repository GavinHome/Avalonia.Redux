namespace samples;

public class CounterPage //:<CounterState, Dictionary<String, dynamic>>
{
    // CounterPage() : base(
    //     initState: () => { },
    //     effect: () => { },
    //     reducer: () => { },
    //     view: (CounterState state, Dispatch dispatch, Context ctx) =>
    //     {
    //x
    //     }
    // );
}
//
// /// init store's state by route-params
// public delegate T InitState<T, P>(P param);
// /// Component's view part
// /// 1.State is used to decide how to render
// /// 2.Dispatch is used to send actions
// /// 3.ViewService is used to build sub-components or adapter.
// public delegate dynamic ViewBuilder<T>(T state, Dispatch dispatch); //ViewService viewService
//
// public abstract class Page<T, P>
// {
//     private InitState<T, P> _initState;
//
//     protected Page(InitState<T, P> initState, ViewBuilder<T> view, Effect<T>? effect, Reducer<T>? reducer)
//     {
//         _initState = initState;
//     }
//
// }

// public class CounterState
// {
//    public int Count => 0;
// }