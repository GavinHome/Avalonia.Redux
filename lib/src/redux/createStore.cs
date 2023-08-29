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