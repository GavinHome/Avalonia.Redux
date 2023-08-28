namespace Redux;

public class StoreCreator
{
    private static Store<T> _createStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware) where T : class, new()
    {
        return new Store<T>(preloadedState, reducer, middleware);
    }

    /// <summary>
    /// Create Store
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="initState">The state object instance.</param>
    /// <param name="reducer"></param>
    /// <returns>The store object</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Store<T> CreateStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware) where T : class, new() =>
        _createStore(preloadedState, reducer, middleware: middleware);
}

public static class StoreExtends
{
    public static Store<object> ObjectClone<T>(this Store<T> src) where T : class, new()
    {
        var obj = src.GetState() as object;
        var store = new Store<object>(obj!, null, null);
        store.Subscribe = src.Subscribe;
        store.Dispatch = src.Dispatch;
        store.ReplaceReducer(reducer: (object state, Action action) => (src.GetReducer ?? ((state, action) => state)).Invoke((T)state, action));
        return store;
    }
}

//class InternalStore<T> : Store<T>
//{
//    private T _state;
//    private IList<System.Action> _listeners;
//    private Reducer<T>? _reducer;

//    public new Get<T> GetState => () => _state;
//    public new Dispatch Dispatch { get; set; }

//    public new Subscribe Subscribe { get; set; }
//    //public Observable<T> Observable { get; set; }
//    public new ReplaceReducer<T> ReplaceReducer { get; private set; }

//    bool isDispatching = false;
//    bool isDisposed = false;

//    public InternalStore(T initState, Reducer<T>? reducer, List<Middleware<T>>? middleware)
//    {
//        _throwIfNot(initState != null, "Expected the preloadedState to be non-null value.");

//        _state = initState;
//        _listeners = new List<System.Action>();
//        _reducer = reducer ?? ((T state, Action action) => state);

//        Dispatch = (action) =>
//        {
//            _throwIfNot(!isDispatching, "Reducers may not dispatch actions.");

//            if (isDisposed)
//            {
//                return null;
//            }

//            try
//            {
//                isDispatching = true;
//                if (this._reducer != null)
//                {
//                    _state = reducer!(_state, action);
//                }
//            }
//            finally
//            {
//                isDispatching = false;
//            }

//            foreach (var listener in _listeners)
//            {
//                listener();
//            };

//            return null;
//        };

//        Dispatch = (middleware != null && middleware.Any())
//            ? middleware!.Select((Middleware<T> middleware) => middleware(
//                dispatch: (Action action) => Dispatch(action),
//                getState: GetState
//            ))
//            .Aggregate(Dispatch, (Dispatch previousValue, Composable<Dispatch> element) => element(previousValue))
//          : Dispatch;

//        ReplaceReducer = (nextReducer) => reducer = nextReducer ?? ((T state, Action action) => state);

//        Subscribe = (listener) =>
//        {
//            _throwIfNot(!isDispatching, "You may not call store.subscribe() while the reducer is executing.");

//            _listeners.Add(listener);
//            return new Unsubscribe(() =>
//            {
//                _throwIfNot(!isDispatching, "You may not unsubscribe from a store listener while the reducer is executing.");
//                _listeners.Remove(listener);
//                return true;
//            });
//        };
//    }

//    void _throwIfNot(bool condition, string? message = null)
//    {
//        if (!condition)
//        {
//            throw new ArgumentException(message);
//        }
//    }
//}
