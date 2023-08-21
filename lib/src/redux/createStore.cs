namespace Redux;

public class StoreCreator
{
    private static Store<T> _createStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware)
    {
        return new InternalStore<T>(preloadedState, reducer, middleware);
    }

    /// <summary>
    /// Create Store
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="initState">The state object instance.</param>
    /// <param name="reducer"></param>
    /// <returns>The store object</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Store<T> CreateStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware) =>
        _createStore(preloadedState, reducer, middleware: middleware);
}

public class InternalStore<T> : Store<T>
{
    private T _state;
    private IList<System.Action> _listeners;
    private Reducer<T>? _reducer;

    public new Get<T> GetState => () => _state;
    public new Dispatch Dispatch { get; set; }

    public new Subscribe Subscribe { get; set; }
    //public Observable<T> Observable { get; set; }
    public new ReplaceReducer<T> ReplaceReducer { get; private set; }

    private Reducer<T> _noop<T>() => (T state, Action action) => state;

    bool isDispatching = false;
    bool isDisposed = false;

    public InternalStore(T initState, Reducer<T>? reducer, List<Middleware<T>>? middleware)
    {
        _state = initState;
        _listeners = new List<System.Action>();
        _reducer = reducer ?? _noop<T>();

        Dispatch = (action) =>
        {
            if (isDisposed)
            {
                return null;
            }

            try
            {
                isDispatching = true;
                if (this._reducer != null)
                {
                    _state = reducer!(_state, action);
                }
            }
            finally
            {
                isDispatching = false;
            }

            foreach (var listener in _listeners)
            {
                listener();
            };

            return null;
        };

        Dispatch = (middleware != null && middleware.Any())
            ? middleware!.Select((Middleware<T> middleware) => middleware(
                dispatch: (Action action) => Dispatch(action),
                getState: GetState
            ))
            .Aggregate(Dispatch, (Dispatch previousValue, Composable<Dispatch> element) => element(previousValue))
          : Dispatch;

        ReplaceReducer = (nextReducer) => reducer = nextReducer ?? _noop<T>();

        Subscribe = (listener) =>
        {
            _listeners.Add(listener);
            return new Unsubscribe(() => _listeners.Remove(listener));
        };
    }
}
