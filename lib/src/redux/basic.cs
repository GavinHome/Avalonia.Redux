namespace Redux;

/// [Action]
/// Effect or Reducer message action
public class Action
{
    private readonly object _type;
    private readonly dynamic? _payload;

    public Action(object type, dynamic? payload = null)
    {
        _type = type;
        _payload = payload;
    }

    public object Type => _type;
    public dynamic? Payload => _payload;

    public override bool Equals(object? obj) => obj != null && Equals(other: obj as Action);

    public bool Equals(Action? other) => other != null && _type == other.Type;

    public override int GetHashCode() => HashCode.Combine(_type);
}

/// [Store]
/// Definition of the standard Store.
public class Store<T> where T : class
{
    //public Get<T> GetState;
    //public Dispatch Dispatch;
    //public Subscribe Subscribe;
    //////public Observable<T> observable;
    //public ReplaceReducer<T> ReplaceReducer;

    private T? _state;
    private IList<System.Action> _listeners;
    private Reducer<T>? _reducer;

    public Get<T> GetState => () => _state!;
    public Reducer<T>? GetReducer => _reducer;
    public Dispatch Dispatch { get; set; }

    public Subscribe Subscribe { get; set; }
    //public Observable<T> Observable { get; set; }
    public ReplaceReducer<T> ReplaceReducer { get; private set; }

    bool isDispatching = false;
    bool isDisposed = false;

    public Store(T initState, Reducer<T>? reducer, List<Middleware<T>>? middleware)
    {
        _throwIfNot(initState != null, "Expected the preloadedState to be non-null value.");

        _state = initState;
        _listeners = new List<System.Action>();
        _reducer = reducer ?? ((T state, Action action) => state);

        Dispatch = (action) =>
        {
            _throwIfNot(!isDispatching, "Reducers may not dispatch actions.");

            if (isDisposed)
            {
                return null;
            }

            try
            {
                isDispatching = true;
                if (this._reducer != null)
                {
                    _state = reducer!(_state!, action);
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

        ReplaceReducer = (nextReducer) => reducer = nextReducer ?? ((T state, Action action) => state);

        Subscribe = (listener) =>
        {
            _throwIfNot(!isDispatching, "You may not call store.subscribe() while the reducer is executing.");

            _listeners.Add(listener);
            return new Unsubscribe(() =>
            {
                _throwIfNot(!isDispatching, "You may not unsubscribe from a store listener while the reducer is executing.");
                _listeners.Remove(listener);
                return true;
            });
        };
    }

    void _throwIfNot(bool condition, string? message = null)
    {
        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }
}

/// [Get]
/// Definition of the function type that returns type R.
public delegate R Get<R>();

/// [Dispatch] patch action function
/// Definition of the standard Dispatch.
/// Send an "intention".
public delegate dynamic? Dispatch(Action action);

/// [Subscribe]
/// Definition of a standard subscription function.
/// input a subscriber and output an unsubscription function.
public delegate Unsubscribe Subscribe(System.Action callback);

/// [Unsubscribe]
/// Definition of a standard un-subscription function.
////public delegate void Unsubscribe();
public class Unsubscribe
{
    public Func<bool> Cancel { get; private set; }

    public Unsubscribe(Func<bool> cancel)
    {
        this.Cancel = cancel;
    }
}

/// [Observable]
/// Definition of the standard observable flow.
//public delegate Stream Observable<T>();

/// [ReplaceReducer]
/// Definition of a standard ReplaceReducer function.
public delegate void ReplaceReducer<T>(Reducer<T>? reducer);

/// Definition of the standard Middleware.
public delegate Composable<Dispatch> Middleware<T>(Dispatch dispatch, Get<T> getState);

/// Definition of synthesizable functions.
//typedef Composable<T> = T Function(T next);
public delegate T Composable<T>(T next);

/// Definition of the standard Reducer.
/// If the Reducer needs to respond to the Action, it returns a new state, otherwise it returns the old state.
public delegate T Reducer<T>(T state, Action action);

/// Definition of SubReducer
/// [isStateCopied] is Used to optimize execution performance.
/// Ensure that a T will be cloned at most once during the entire process.
public delegate T SubReducer<T>(T state, Action action, bool isStateCopied);

public static class ReducerConverter
{
    /// [asReducer]
    /// combine & as
    /// for action.type which override it's == operator
    public static Reducer<T> asReducers<T>(Dictionary<object, Reducer<T>>? map)
    {
        if (map == null || !map.Any())
        {
            return (state, action) => state;
        }
        else
        {
            return (state, action) =>
            {
                var fn = map.FirstOrDefault(entry => action.Type.Equals(entry.Key)).Value;
                if (fn != null)
                {
                    return fn(state, action);
                }

                else return state;
            };
        }
    }

    /// [CombineReducers]
    /// Combine an iterable of SubReducer<T> into one Reducer<T>
    public static Reducer<T>? CombineReducers<T>(IList<Reducer<T>>? reducers)
    {
        var notNullReducers = reducers?.ToArray();
        if (notNullReducers == null || !notNullReducers.Any())
        {
            return null;
        }

        if (notNullReducers.Length == 1)
        {
            return notNullReducers.Single();
        }

        return (T state, Action action) =>
        {
            T nextState = state;
            foreach (Reducer<T> reducer in notNullReducers)
            {
                nextState = reducer(nextState, action);
            }

            return nextState;
        };
    }

    /// [CombineSubReducers]
    /// Combine an iterable of SubReducer<T> into one Reducer<T>
    public static Reducer<T>? CombineSubReducers<T>(IList<SubReducer<T>> subReducers)
    {
        var notNullReducers = subReducers?.ToArray();
        if (notNullReducers == null || !notNullReducers.Any())
        {
            return null;
        }

        if (notNullReducers.Length == 1)
        {
            SubReducer<T> single = notNullReducers.Single();
            return (T state, Action action) => single(state, action, false);
        }

        return (T state, Action action) =>
        {
            T copy = state;
            bool hasChanged = false;
            foreach (SubReducer<T> subReducer in notNullReducers)
            {
                copy = subReducer(copy, action, hasChanged);
                hasChanged = hasChanged || !EqualityComparer<T>.Default.Equals(copy, state); //copy != state
            }

            return copy;
        };
    }
}