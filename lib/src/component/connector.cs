// ReSharper disable ConvertClosureToMethodGroup
namespace Redux.Component;
using Widget = Avalonia.Controls.Control;

//// Definition of Connector which connects Reducer<S> with Reducer<P>.
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
    public abstract SubReducer<S>? subReducer(Reducer<P>? reducer);
}

/// [MutableConn]
/// Definition of MutableConn for page and component(s).
public abstract class MutableConn<T, P> : AbstractConnector<T, P>
{
    protected abstract void Set(T state, P subState);

    public override SubReducer<T>? subReducer(Reducer<P>? reducer)
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

    /// [ConnOp]
    /// Mixin of Connector for Component
    public static Dependent<T> operator +(MutableConn<T, P> connector, BasicComponent<P> component)
    {
        return connector.createDependent(component);
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
    public override T Get(T state) => state;

    protected override void Set(T state, T subState) { }
}

/// [ConnOp]
/// DThe implementation of connector.
public class ConnOp<T, P> : MutableConn<T, P>
{
    private readonly Func<T, P>? _getter;
    private readonly Action<T, P>? _setter;

    public ConnOp(Func<T, P> getter, Action<T, P> setter)
    {
        _getter = getter;
        _setter = setter;
    }

    protected ConnOp() { }

    public override P Get(T state) => _getter!(state);

    protected override void Set(T state, P subState) => _setter!(state, subState);
}

/// [_Dependent]
/// Implementation of Dependent
public class _Dependent<T, P> : Dependent<T>
{
    readonly MutableConn<T, P> _connector;
    readonly SubReducer<T>? _subReducer;
    readonly Reducer<P> _reducer;
    readonly BasicComponent<P> _component;

    public _Dependent(BasicComponent<P> component, MutableConn<T, P> connector)
    {
        _connector = connector;
        _component = component;
        _reducer = component.createReducer();
        _subReducer = _conn((state, action) => _reducer(state, action), connector);
    }

    SubReducer<T>? _conn(Reducer<P>? reducer, MutableConn<T, P> connector)
    {
        return reducer == null
            ? null
            : connector.subReducer(
                (state, action) => reducer(state, action));
    }

    public override Widget buildComponent(Store<object> store, Get<T> getter)
    {
        return _component.buildComponent(store, () => _connector.Get(getter()));
    }

    public override List<Widget> buildComponents(Store<object> store, Get<T> getter)
    {
        return _component.buildComponents(store, () => _connector.Get(getter()));
    }

    // ReSharper disable once UnusedParameter.Local
    public override SubReducer<T> createSubReducer() => _subReducer ?? ((T state, Action _, bool __) => state);

    public override ComponentBase<object> Component => (_component as BasicComponent<object>)!;
}

static class DependentExtends
{
    public static Dependent<K> createDependent<K, T>(this MutableConn<K, T> connector, BasicComponent<T> component) =>
        new _Dependent<K, T>(connector: connector, component: component);
}
