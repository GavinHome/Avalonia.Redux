namespace Redux;

public static class StoreCreator
{
    private static Store<T> _createStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware) //where T : class, new()
    {
        return new Store<T>(preloadedState, reducer, middleware);
    }

    /// <summary>
    /// Create Store
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="preloadedState">The state object instance.</param>
    /// <param name="reducer"></param>
    /// <param name="middleware"></param>
    /// <returns>The store object</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Store<T> CreateStore<T>(T preloadedState, Reducer<T>? reducer, List<Middleware<T>>? middleware) //where T : class, new() 
        => _createStore(preloadedState, reducer, middleware: middleware);
}

public static class StoreExtends
{
    public static Store<object> ObjectClone<T>(this Store<T> src) //where T : class, new()
    {
        var obj = src.GetState() as object;
        var store = new Store<object>(obj, null, null)
        {
            Subscribe = src.Subscribe,
            Dispatch = src.Dispatch
        };
        store.ReplaceReducer(reducer: (state, action) => (src.GetReducer ?? ((_state, _) => _state)).Invoke((T)state, action));
        return store;
    }
}