namespace Redux.Component;

/// Definition of Connector which connects Reducer<S> with Reducer<P>.
/// 1. How to get an instance of type P from an instance of type S.
/// 2. How to synchronize changes of an instance of type P to an instance of type S.
/// 3. How to clone a new S.
public abstract class AbstractConnector<S, P>
{
    public abstract P Get(S state);

    /// For mutable state, there are three abilities needed to be met.
    ///     1. get: (S) => P
    ///     2. set: (S, P) => void
    ///     3. shallow copy: s.clone()
    ///
    /// For immutable state, there are two abilities needed to be met.
    ///     1. get: (S) => P
    ///     2. set: (S, P) => S
    ///
    /// See in [connector].
    public abstract SubReducer<S>? subReducer(Reducer<P> reducer);
}

/// [MutableConn]
/// Definition of MutableConn for page and component(s).
public abstract class MutableConn<T, P> : AbstractConnector<T, P>
{
    public MutableConn() { }

    public abstract void Set(T state, P subState);

    public override SubReducer<T>? subReducer(Reducer<P> reducer)
    {
        if (reducer == null)
        {
            return null;
        }
        else
        {
            return (state, action, isStateCopied) =>
            {
                P props = Get(state);
                if (props == null)
                {
                    return state;
                }

                P newProps = reducer(props, action);
                bool hasChanged = !EqualityComparer<P>.Default.Equals(newProps, props); //newProps != props
                T copy = hasChanged && !isStateCopied ? _clone(state) : state;
                if (hasChanged)
                {
                    Set(copy, newProps);
                }

                return copy;
            };
        }
    }

    /// how to clone an object
    T _clone(T state)
    {
        if (state is ICloneable || state is object || state is List<object> || state is Dictionary<string, dynamic>)
        {
            return state.Clone()!;
        }
        else if (state == null)
        {
            return default!;
        }
        else
        {
            throw new ArgumentException($"Could not clone this state of type {typeof(T)}");
        }
    }
}

/// [NoneConn]
/// The implementation of connector.
public class NoneConn<T> : ConnOp<T, T>
{
    public NoneConn() { }

    public override T Get(T state) => state;

    public override void Set(T state, T subState) { }
}

/// [ConnOp]
/// DThe implementation of connector.
public class ConnOp<T, P> : MutableConn<T, P>
{
    private Func<T, P>? _getter;
    private Action<T, P>? _setter;

    public ConnOp(Func<T, P> getter, Action<T, P> setter)
    {
        _getter = getter;
        _setter = setter;
    }

    public ConnOp() { }

    public override P Get(T state) => _getter!(state);

    public override void Set(T state, P subState) => _setter!(state, subState);

    //public Dependent<T> add(AbstractLogic<P> logic) =>
    //   Redux.DependentCreator.createDependent(this, logic);
}
