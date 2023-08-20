using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Redux;

public class Action
{
    private readonly object _type;
    private readonly dynamic? _payload;

    public Action(object type, dynamic? payload)
    {
        _type = type;
        _payload = payload;
    }

    public Action(object type)
    {
        _type = type;
    }

    public object Type { get { return _type; } }
    public dynamic? Payload { get { return _payload; } }

    public override bool Equals(object? obj) => obj != null && Equals(other: obj as Action);

    public bool Equals(Action? other) => other != null && _type == other.Type;

    public override int GetHashCode() => HashCode.Combine(_type);
}

/// [Store]
/// Definition of the standard Store.
public class Store<T>
{
    private T _state;
    private IList<System.Action> _listeners;
    private Reducer<T> _reducer;

    public Get<T> GetState => () => _state;
    public Dispatch Dispatch { get; set; }

    public Subscribe Subscribe { get; set; }
    ////public Observable<T> Observable { get; set; }
    public ReplaceReducer<T> ReplaceReducer { get; private set; }

    public Store(T initState, Reducer<T> reducer)
    {
        _state = initState;
        _listeners = new List<System.Action>();
        _reducer = reducer;

        Dispatch = (action) =>
        {
            if (this._reducer != null)
            {
                _state = reducer(_state, action);

                foreach (var listener in _listeners)
                {
                    listener();
                };
            }
        };

        Subscribe = (listener) =>
        {
            _listeners.Add(listener);
            return new Unsubscribe(() => _listeners.Remove(listener));
        };

        ReplaceReducer = (nextReducer) => reducer = nextReducer;
    }
}


/// Definition of the function type that returns type R.
public delegate R Get<R>();

/// Definition of the standard Dispatch.
/// Send an "intention".
public delegate void Dispatch(Action action);

/// Definition of a standard subscription function.
/// input a subscriber and output an anti-subscription function.
public delegate Unsubscribe Subscribe(System.Action callback);

///// Definition of a standard un-subscription function.
//public delegate void Unsubscribe();
public class Unsubscribe
{
    public Func<bool> Cancel { get; private set; }

    public Unsubscribe(Func<bool> cancel)
    {
        this.Cancel = cancel;
    }
}

/////// Definition of the standard observable flow.
//public delegate Stream Observable<T>();

/// Definition of ReplaceReducer
public delegate void ReplaceReducer<T>(Reducer<T> reducer);

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

/// Create a store definition
// public delegate Store<T> StoreCreator<T>(
//     T preloadedState,
//     Reducer<T> reducer
// );

/// Definition of Enhanced creating a store
// public delegate StoreCreator<T> StoreEnhancer<T>(StoreCreator<T> creator);


// /// Definition of Connector which connects Reducer<S> with Reducer<P>.
// /// 1. How to get an instance of type P from an instance of type S.
// /// 2. How to synchronize changes of an instance of type P to an instance of type S.
// /// 3. How to clone a new S.
// public abstract class AbstractConnector<S, P>
// {
//     public abstract P Get(S state);
//
//     /// For mutable state, there are three abilities needed to be met.
//     ///     1. get: (S) => P
//     ///     2. set: (S, P) => void
//     ///     3. shallow copy: s.clone()
//     ///
//     /// For immutable state, there are two abilities needed to be met.
//     ///     1. get: (S) => P
//     ///     2. set: (S, P) => S
//     ///
//     /// See in [connector].
//     public abstract SubReducer<S> subReducer(Reducer<P> reducer);
// }

public static class ReducerConverter
{
    public static Reducer<T> AsReducers<T>(Dictionary<object, Reducer<T>>? map)
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

        return (T state, Redux.Action action) =>
        {
            T nextState = state;
            foreach (Reducer<T> reducer in notNullReducers)
            {
                nextState = reducer(nextState, action);
            }

            return nextState;
        };
    }

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
            return (T state, Redux.Action action) => single(state, action, false);
        }

        return (T state, Redux.Action action) =>
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