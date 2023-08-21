namespace Redux;

public class StoreCreator
{
    /// <summary>
    /// Create Store
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="initState">The state object instance.</param>
    /// <param name="reducer"></param>
    /// <returns>The store object</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Store<T> CreateStore<T>(T initState, Reducer<T> reducer)
    {
        Store<T> store = new Store<T>(initState, reducer);
        return store;
    }

    // /// create a store with enhancer
    // public static Store<T> createStore<T>(T preloadedState, Reducer<T> reducer, StoreEnhancer<T>? enhancer)
    // {
    //     return enhancer != null ? enhancer(createStore)(preloadedState, reducer) : createStore(preloadedState, reducer);
    // }
    //
    // public static Store<T> createStore<T>(T preloadedState, Reducer<T> reducer, StoreEnhancer<T>? enhancer,
    //     Dependencies<T>? dependencies)
    // {
    //     var reducers = new List<Reducer<T>>() { reducer };
    //     if (dependencies != null)
    //     {
    //         reducers.Add(dependencies.createReducer());
    //     }
    //
    //     var combineReducers = ReducerCreator.combineReducers(reducers);
    //     return enhancer != null
    //         ? enhancer(createStore)(preloadedState, combineReducers)
    //         : createStore(preloadedState, reducer);
    // }
}

//// Create a store definition
// public delegate Store<T> StoreCreator<T>(
//     T preloadedState,
//     Reducer<T> reducer
// );