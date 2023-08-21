// ReSharper disable CheckNamespace
namespace Redux.Component;

/// Predicate if a component should be updated when the store is changed.
public delegate bool ShouldUpdate<T>(T old, T now);

public abstract class ComponentBase<T> { }

/// Representation of each dependency
public abstract class Dependent<T>
{
    /// to building a normal component, it return a widget.
    /// but throw a exception to use building adapter which composed-component
    public abstract Widget buildComponent(Store<Object> store, Get<T> getter);

    /// to building adapter which composed-component, it return a list of widget.
    public abstract List<Widget> buildComponents(Store<Object> store, Get<T> getter);

    public abstract SubReducer<T> createSubReducer();
}

public class Dependencies<T>
{
    IDictionary<String, Dependent<T>>? slots;
    Dependent<T>? adapter;

    /// Use [adapter: NoneConn<T>() + Adapter<T>(),
    ///       slots: <String, Dependent<P>> {
    ///         ConnOp<T, P>() + Component<T>()}
    ///     ],
    /// Which is better reusability and consistency.
    public Dependencies(IDictionary<String, Dependent<T>>? slots, Dependent<T> adapter)
    {
        this.slots = slots;
        this.adapter = adapter;
    }

    public Dependencies(IDictionary<String, Dependent<T>>? slots)
    {
        this.slots = slots;
    }

    public Dependencies(Dependent<T> adapter)
    {
        this.adapter = adapter;
    }

    public Reducer<T> createReducer()
    {
        List<SubReducer<T>> subs = new List<SubReducer<T>>();
        if (slots != null && slots.Any())
        {
            subs.AddRange(slots.Values.Select((Dependent<T> entry) => entry.createSubReducer()).ToList());
        }

        if (adapter != null)
        {
            subs.Add(adapter.createSubReducer());
        }

        Reducer<T> _noop<T>() => (T state, Action action) => state;
        var subReduces = ReducerConverter.CombineSubReducers(subs);
        return ReducerConverter.CombineReducers(new List<Reducer<T>> { subReduces ?? _noop<T>() }) ?? _noop<T>();
    }

    public Dependent<T>? slot(String type) => slots?[type];

    public Dependencies<T>? trim() => (adapter != null || (slots?.Any() ?? false)) ? this : null;
}

public class ComponentContext<T>
{
    private ViewBuilder<T>? view;
    private Dependencies<T>? dependencies;
    private Effect<T>? effect;
    private Store<object> store;
    private Get<T> getState;
    private MarkNeedsBuild? markNeedsBuild;
    private ShouldUpdate<T> shouldUpdate;

    public ComponentContext(Store<object> store, Get<T> getState, Dependencies<T>? dependencies, MarkNeedsBuild? markNeedsBuild, ViewBuilder<T>? view, Effect<T>? effect, ShouldUpdate<T>? shouldUpdate)
    {
        this.store = store;
        this.getState = getState;
        this.markNeedsBuild = markNeedsBuild;
        this.dependencies = dependencies;
        this.view = view;
        this.effect = effect;
        this.shouldUpdate = shouldUpdate ?? _updateByDefault<T>();
    }

    private ShouldUpdate<K> _updateByDefault<K>() =>
        (K _, K __) => !EqualityComparer<K>.Default.Equals(_, __);//!identical(_, __);
}

/// Component's view part
/// 1.State is used to decide how to render
/// 2.Dispatch is used to send actions
/// 3.ComponentContext is used to build sub-components or adapter.
public delegate dynamic ViewBuilder<T>(T state, Dispatch dispatch, ComponentContext<T> ctx);

/// Interrupt if not null not false
/// bool for sync-functions, interrupted if true
/// Future for async-functions, should always be interrupted.
public delegate dynamic? Effect<T>(Action action, ComponentContext<T> ctx);

public delegate Task SubEffect<T>(Action action, ComponentContext<T> ctx);

public static class EffectConverter
{
    static readonly object SubEffectReturnNull = new object();

    public static Effect<T>? CombineEffects<T>(Dictionary<object, SubEffect<T>>? map) => map == null || !map.Any()
        ? null : (action, ctx) =>
        {
            SubEffect<T> subEffect = map.FirstOrDefault(entry => action.Type.Equals(entry.Key)).Value;
            if (subEffect != null)
            {
                return (subEffect.Invoke(action, ctx) ?? SubEffectReturnNull) == null;
            }

            ////no subEffect
            return null;
        };
}

public abstract class Widget
{
}

public delegate bool MarkNeedsBuild();

public abstract class AbstractComponent<T> : ComponentBase<T>
{
    private Dependencies<T>? _dependencies;
    private Reducer<T> _reducer;
    private Effect<T>? _effect;
    private ViewBuilder<T>? _view;
    private ShouldUpdate<T>? _shouldUpdate;

    protected AbstractComponent(Reducer<T> reducer, ViewBuilder<T>? view, Effect<T>? effect, Dependencies<T>? dependencies, ShouldUpdate<T>? shouldUpdate)
    {
        _reducer = reducer;
        _effect = effect;
        _view = view;
        _dependencies = dependencies;
        _shouldUpdate = shouldUpdate;
    }

    ComponentContext<T> CreateContext(Store<Object> store, Get<T> getter, MarkNeedsBuild markNeedsBuild)
    {
        return new ComponentContext<T>(
            store: store,
            //bus: bus,
            getState: getter,
            markNeedsBuild: markNeedsBuild,
            dependencies: _dependencies,
            view: _view,
            effect: _effect,
            //buildContext: buildContext,
            shouldUpdate: _shouldUpdate
        );
    }

    public abstract Widget buildComponent(Store<Object> store, Get<T> getter);

    public abstract List<Widget> buildComponents(Store<Object> store, Get<T> getter);
}
