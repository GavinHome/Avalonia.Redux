namespace Redux.Component;

public abstract class ComponentElement : Control
{
    public abstract Widget build();
}

public class StatefulElement : ComponentElement
{
    public StatefulElement(StatefulWidget widget)
    {
        _state = widget.createState();
        ////state._element = this;
        state._widget = widget;
        _state.initState();
    }

    public override Widget build() => state.build(this);

    State<StatefulWidget> state => _state!;
    readonly State<StatefulWidget>? _state;

    #region 
    ////private bool _dirty;
    ////private bool dirty => _dirty;

    ////internal void markNeedsBuild()
    ////{
    ////    if (dirty)
    ////    {
    ////        return;
    ////    }

    ////    _dirty = true;
    ////    ////state.build(this);
    ////    rebuild();
    ////}

    ////void rebuild(bool force = false)
    ////{
    ////    if ((!_dirty && !force))
    ////    {
    ////        return;
    ////    }

    ////    _dirty = false;
    ////}
    #endregion
}

public abstract class StatefulWidget : Widget
{
    public abstract State<StatefulWidget> createState();
    public Widget create() => new StatefulElement(this).build();
}

/// [VoidCallback]
public delegate dynamic? VoidCallback();

/// [State<T>]
public abstract class State<T> where T : StatefulWidget
{
    public T widget => _widget!;
    public T? _widget;

    ////public Widget? _element;
    ////public bool mounted => _element != null;

    public virtual void initState() { }

    public abstract Widget build(dynamic context);

    protected virtual void didChangeDependencies() { }

    protected virtual void deactivate() { }

    protected virtual void reassemble() { }

    protected virtual void didUpdateWidget(T oldWidget) { }

    protected virtual void disposeCtx() { }

    protected virtual void dispose() { }

    public void setState(VoidCallback fn)
    {
        object? _ = fn();
        ////_element!.markNeedsBuild();
    }
}

/// Log
static class Log
{
    public static void doPrint(object message)
    {
        Action<Object> print = obj => Console.WriteLine($"[AvaloniaRedux]: {obj}");
        if (Aop.isDebug())
        {
            print(message);
        }
    }
}

/// Predicate if a component should be updated when the store is changed.
public delegate bool ShouldUpdate<T>(T? old, T? now);

/// [ComponentBase]
/// Definition of the component base class.
public abstract class ComponentBase<T> { }

/// [Dependent]
/// Definition of the dependent for adapter and slot.
public abstract class Dependent<T>
{
    /// to building a normal component, it return a widget.
    /// but throw a exception to use building adapter which composed-component
    public abstract Widget buildComponent(Store<object> store, Get<T> getter);

    /// to building adapter which composed-component, it return a list of widget.
    public abstract List<Widget> buildComponents(Store<object> store, Get<T> getter);

    public abstract SubReducer<T> createSubReducer();

    /// component getter
    public abstract ComponentBase<object> Component { get; }
}

/// [Dependencies]
/// Definition of the Dependencies for page or component.
/// Include adapter and slots.
public class Dependencies<T>
{
    private readonly IDictionary<string, Dependent<T>>? slots;
    public readonly Dependent<T>? adapter;

    //// Use [adapter: NoneConn<T>() + Adapter<T>(),
    ////       slots: <String, Dependent<P>> {
    ////         ConnOp<T, P>() + Component<T>()}
    ////     ],
    //// Which is better reusability and consistency.
    public Dependencies(IDictionary<string, Dependent<T>>? slots, Dependent<T> adapter)
    {
        this.slots = slots;
        this.adapter = adapter;
    }

    public Dependencies(IDictionary<string, Dependent<T>>? slots)
    {
        this.slots = slots;
    }

    public Dependencies(Dependent<T> adapter)
    {
        this.adapter = adapter;
    }

    public Reducer<T> createReducer()
    {
        List<SubReducer<T>> subs = new();
        if (slots != null && slots.Any())
        {
            subs.AddRange(slots.Values.Select(entry => entry.createSubReducer()).ToList());
        }

        if (adapter != null)
        {
            subs.Add(adapter.createSubReducer());
        }

        var subReduces = ReducerConverter.CombineSubReducers(subs);
        return ReducerConverter.CombineReducers(new List<Reducer<T>> { subReduces ?? ((T state, Action _) => state) }) ?? ((T state, Action _) => state);
    }

    public Dependent<T>? slot(string type) => slots?[type];

    public Dependencies<T>? trim() => (adapter != null || (slots?.Any() ?? false)) ? this : null;
}

/// [Lifecycle]
public enum Lifecycle
{
    /// component(page) or adapter receives the following events
    initState,
    didChangeDependencies,
    build,
    reassemble,
    didUpdateWidget,
    deactivate,
    dispose,
    // didDisposed,

    //// Only a adapter mixin VisibleChangeMixin will receive appear & disappear events.
    //// class MyAdapter extends Adapter<T> with VisibleChangeMixin<T> {
    ////   MyAdapter():super(
    ////   );
    //// }
    appear,
    disappear,

    //// Only a component(page) or adapter mixin WidgetsBindingObserverMixin will receive didChangeAppLifecycleState event.
    //// class MyComponent extends Component<T> with WidgetsBindingObserverMixin<T> {
    ////   MyComponent():super(
    ////   );
    //// }
    didChangeAppLifecycleState,
}

static class LifecycleCreator
{
    public static Action initState() => new(Lifecycle.initState);

    public static Action build(string name) => new(Lifecycle.build, payload: name);

    public static Action reassemble() => new(Lifecycle.reassemble);

    public static Action dispose() => new(Lifecycle.dispose);

    // static Action didDisposed() => const Action(Lifecycle.didDisposed);

    public static Action didUpdateWidget() => new(Lifecycle.didUpdateWidget);

    public static Action didChangeDependencies() => new(Lifecycle.didChangeDependencies);

    public static Action deactivate() => new(Lifecycle.deactivate);

    public static Action appear(int index) => new(Lifecycle.appear, payload: index);

    public static Action disappear(int index) => new(Lifecycle.disappear, payload: index);
}

/// [ComponentContext]
/// Definition context of component or page.
public class ComponentContext<T>
{
    private readonly ViewBuilder<T>? view;
    private readonly Dependencies<T>? _dependencies;
    private readonly Effect<T>? effect;
    public readonly Store<object> store;
    private readonly Get<T> getState;
    private readonly MarkNeedsBuild? markNeedsBuild;
    private readonly ShouldUpdate<T> _shouldUpdate;
    Dispatch? _dispatch;
    Dispatch? _effectDispatch;

    public ComponentContext(Store<object> store, Get<T> getState, Dependencies<T>? dependencies = null, MarkNeedsBuild? markNeedsBuild = null, ViewBuilder<T>? view = null, Effect<T>? effect = null, ShouldUpdate<T>? shouldUpdate = null)
    {
        this.store = store;
        this.getState = getState;
        this.markNeedsBuild = markNeedsBuild;
        _dependencies = dependencies;
        this.view = view;
        this.effect = effect;
        _shouldUpdate = shouldUpdate ?? _updateByDefault<T>();

        _init();
    }

    public T state => getState();
    Widget? _widgetCache;
    T? _latestState;

    public Dispatch Dispatch => _dispatch!;
    //  BuildContext get context => buildContext!;

    public Widget buildView()
    {
        Widget? result = _widgetCache;
        if (result == null)
        {
            dispatch(LifecycleCreator.build(""));
        }
        result ??= _widgetCache = view!.Invoke(getState(), dispatch, this);
        return result;
    }

    dynamic? dispatch(Action action) => _dispatch?.Invoke(action);
    //void broadcastEffect(Action action, bool? excluded) => _bus
    //    ?.dispatch(action, excluded: excluded == true ? _effectDispatch : null);

    public Widget buildComponent(string type)
    {
        Dependent<T>? dependent = _dependencies?.slot(type);
        return dependent!.buildComponent(
          store,
          getState
        );
    }

    public List<Widget> buildComponents()
    {
        Dependent<T> dependent = _dependencies!.adapter!;
        return dependent.buildComponents(
          store,
          getState
        );
    }

    //    Function()? _dispatchDispose;

    Dispatch _createNextDispatch(ComponentContext<T> ctx) => action =>
    {
        ctx.store.Dispatch.Invoke(action);
        return null;
    };

    void _init()
    {
        _effectDispatch = _createEffectDispatch(effect, this);
        _dispatch =
            _createDispatch(_effectDispatch, _createNextDispatch(this), this);
        //_dispatchDispose = _bus!.registerReceiver(_effectDispatch);
        _latestState = getState();
    }

    public void dispose()
    {
        //_dispatchDispose?.call();
        //_dispatchDispose = null;
    }

    public void onNotify()
    {
        T now = state;
        markNeedsBuild?.Invoke();
        if (_shouldUpdate(_latestState, now))
        {
            _widgetCache = null;
            markNeedsBuild?.Invoke();
            _latestState = now;
        }
    }

    public void didUpdateWidget()
    {
        T now = state;
        if (_shouldUpdate(_latestState, now))
        {
            _widgetCache = null;
            _latestState = now;
        }
    }

    public void onLifecycle(Action action)
    {
        effect?.Invoke(action, this);
    }

    public void clearCache()
    {
        _widgetCache = null;
    }

    ///// return [EffectDispatch]
    Dispatch _createEffectDispatch(
        Effect<T>? userEffect, ComponentContext<T> ctx)
    {
        return action =>
        {
            object? result = userEffect?.Invoke(action, ctx);

            //skip-lifecycle-actions
            if (action.Type is Lifecycle && (result is null || !(result as bool?).GetValueOrDefault()))
            {
                return new object();
            }

            return result;
        };
    }

    Dispatch _createDispatch(
            Dispatch onEffect, Dispatch next, ComponentContext<T> _) =>
        action =>
        {
            object? result = onEffect.Invoke(action);
            if (result is null || !(result as bool?).GetValueOrDefault())
            {
                next(action);
            }

            return result == new object() ? null : result;

        };

    private ShouldUpdate<K> _updateByDefault<K>() =>
                (k, x) => !EqualityComparer<K>.Default.Equals(k, x);//!identical(_, __);
}

/// [ViewBuilder]
/// Component's view part
/// 1.State is used to decide how to render
/// 2.Dispatch is used to send actions
/// 3.ComponentContext is used to build sub-components or adapter.
public delegate dynamic ViewBuilder<T>(T state, Dispatch dispatch, ComponentContext<T> ctx);

/// [Effect] is the definition of the side effect function.
/// According to the return value, determine whether the Action event is consumed.
public delegate dynamic? Effect<T>(Action action, ComponentContext<T> ctx);

/// [SubEffect]
public delegate Task SubEffect<T>(Action action, ComponentContext<T> ctx);

/// [EffectConverter]
public static class EffectConverter
{
    static readonly object SubEffectReturnNull = new();

    /// [combineEffects]
    /// for action.type which override it's == operator
    /// return [UserEffect]
    public static Effect<T>? CombineEffects<T>(Dictionary<object, SubEffect<T>>? map) => map == null || !map.Any()
        ? null : (action, ctx) =>
        {
            SubEffect<T>? subEffect = map.FirstOrDefault(entry => action.Type.Equals(entry.Key)).Value;
            if (subEffect != null)
            {
                return (subEffect.Invoke(action, ctx) ?? SubEffectReturnNull) == null;
            }

            ////no subEffect
            return null;
        };
}

public delegate void MarkNeedsBuild();

/// [AbstractComponent]
/// Definition of basic component.
public abstract class BasicComponent<T> : ComponentBase<T>
{
    protected readonly Dependencies<T>? _dependencies;
    private readonly Reducer<T> _reducer;
    private readonly Effect<T>? _effect;
    private readonly ViewBuilder<T>? _view;
    private readonly ShouldUpdate<T>? _shouldUpdate;

    protected BasicComponent(Reducer<T> reducer, ViewBuilder<T>? view = null, Effect<T>? effect = null, Dependencies<T>? dependencies = null, ShouldUpdate<T>? shouldUpdate = null)
    {
        _reducer = reducer;
        _effect = effect;
        _view = view;
        _dependencies = dependencies;
        _shouldUpdate = shouldUpdate;
    }

    public virtual Reducer<T> createReducer()
    {
        return ReducerConverter.CombineReducers(new List<Reducer<T>> { _reducer, _dependencies?.createReducer() ?? ((T state, Action _) => state) })
            ?? ((T state, Action _) => state);
    }

    public ComponentContext<T> createContext(Store<object> store, Get<T> getter, MarkNeedsBuild? markNeedsBuild = null)
    {
        return new ComponentContext<T>(
            store: store,
            getState: getter,
            markNeedsBuild: markNeedsBuild,
            dependencies: _dependencies,
            view: _view,
            effect: _effect,
            shouldUpdate: _shouldUpdate
        );
    }

    public abstract Widget buildComponent(Store<object> store, Get<T> getter);

    public abstract List<Widget> buildComponents(Store<object> store, Get<T> getter);
}

/// [ComposedComponent]
/// Definition of composed component, only building a list widgets.
public abstract class ComposedComponent<T> : BasicComponent<T>
{
    protected ComposedComponent(
       Reducer<T> reducer, ShouldUpdate<T>? shouldUpdate = null)
      : base(reducer: reducer, view: null, shouldUpdate: shouldUpdate) { }

    public override Widget buildComponent(Store<object> store, Get<T> getter)
    {
        throw new NotImplementedException("ComposedComponent could not build single component");
    }
}

/// [Dependents]
/// Definition of Dependents type, a list of dependent.
public delegate List<Dependent<T>> Dependents<T>(T t);

/// [BasicAdapter]
/// Definition of basic adapter, it is composed component.
/// only building a list widgets.
public class BasicAdapter<T> : ComposedComponent<T>
{
    ComponentContext<T>? _ctx;
    readonly Dependents<T> builder;

    protected BasicAdapter(
        Dependents<T> builder,
        Reducer<T>? reducer = null,
        ShouldUpdate<T>? shouldUpdate = null
      ) : base(
            reducer: reducer ?? ((T state, Action _) => state),
            shouldUpdate: shouldUpdate
          )
    { this.builder = builder; }

    Reducer<T> _createAdapterReducer() => (state, action) =>
    {
        T copy = state;
        bool hasChanged = false;
        List<Dependent<T>> list = builder.Invoke(state);
        foreach (var dep in list)
        {
            SubReducer<T>? subReducer = dep?.createSubReducer();
            if (subReducer != null)
            {
                copy = subReducer(copy, action, hasChanged);
                hasChanged = hasChanged || !EqualityComparer<T>.Default.Equals(copy, state); //copy != state;
            }
        }
        return copy;
    };

    public override Reducer<T> createReducer()
    {
        return ReducerConverter.CombineReducers(new List<Reducer<T>>
        {
            base.createReducer(), _createAdapterReducer()
        }) ?? ((T state, Action _) => state);
    }

    public override List<Widget> buildComponents(Store<object> store, Get<T> getter)
    {
        _ctx ??= createContext(
          store,
          getter,
          markNeedsBuild: () =>
          {
              Log.doPrint($"{GetType()} do reload");
          });
        List<Dependent<T>> dependentArray = builder(getter());
        List<Widget> widgets = new();
        foreach (var dependent in dependentArray)
        {
            widgets.AddRange(
                dependent.buildComponents(
                    store,
                    getter)
            );
        }
        _ctx.onLifecycle(LifecycleCreator.initState());
        return widgets;
    }
}

/// [ObjectCopier]
public static class ObjectCopier
{
    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static T Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", nameof(source));
        }

#pragma warning disable CS8603 // 可能返回 null 引用。
        // Don't serialize a null object, simply return the default for that object
        if (source is null) //if (ReferenceEquals(source, null))
        {
            return default;
        }

        var stream = System.Text.Json.JsonSerializer.Serialize<T>(source);
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream);
#pragma warning restore CS8603 // 可能返回 null 引用。
    }
}