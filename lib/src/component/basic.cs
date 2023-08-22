// ReSharper disable CheckNamespace
namespace Redux.Component;

public abstract class Widget
{
    //public abstract Widget buildWidget();
}

public abstract class StatefulWidget : Widget
{
    public abstract State<StatefulWidget> createState();
    //public abstract State createState();

}

public abstract class State<T> where T : StatefulWidget
{
    public T widget => _widget!;
    protected T? _widget;

    protected virtual void initState() { }

    public abstract Widget build(dynamic context);

    public virtual void didChangeDependencies() { }

    public virtual void deactivate() { }

    public virtual void reassemble() { }

    public virtual void didUpdateWidget(T oldWidget) { }

    public virtual void disposeCtx() { }

    public virtual void dispose() { }
}

/// Log
class Log
{
    public static void doPrint(object message)
    {
        System.Action<object> print = (object obj) => Console.WriteLine(obj);
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
}

/// [Dependencies]
/// Definition of the Dependencies for page or component.
/// Include adapter and slots.
public class Dependencies<T>
{
    IDictionary<string, Dependent<T>>? slots;
    public Dependent<T>? adapter;

    /// Use [adapter: NoneConn<T>() + Adapter<T>(),
    ///       slots: <String, Dependent<P>> {
    ///         ConnOp<T, P>() + Component<T>()}
    ///     ],
    /// Which is better reusability and consistency.
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
        List<SubReducer<T>> subs = new List<SubReducer<T>>();
        if (slots != null && slots.Any())
        {
            subs.AddRange(slots.Values.Select((Dependent<T> entry) => entry.createSubReducer()).ToList());
        }

        if (adapter != null)
        {
            subs.Add(adapter.createSubReducer());
        }

        var subReduces = ReducerConverter.CombineSubReducers(subs);
        return ReducerConverter.CombineReducers(new List<Reducer<T>> { subReduces ?? ((T state, Action action) => state) }) ?? ((T state, Action action) => state);
    }

    public Dependent<T>? slot(string type) => slots?[type];

    public Dependencies<T>? trim() => (adapter != null || (slots?.Any() ?? false)) ? this : null;
}

/// [Lifecycle]
enum Lifecycle
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

    /// Only a adapter mixin VisibleChangeMixin will receive appear & disappear events.
    /// class MyAdapter extends Adapter<T> with VisibleChangeMixin<T> {
    ///   MyAdapter():super(
    ///   );
    /// }
    appear,
    disappear,

    /// Only a component(page) or adapter mixin WidgetsBindingObserverMixin will receive didChangeAppLifecycleState event.
    /// class MyComponent extends Component<T> with WidgetsBindingObserverMixin<T> {
    ///   MyComponent():super(
    ///   );
    /// }
    didChangeAppLifecycleState,
}

class LifecycleCreator
{
    public static Action initState() => new Action(Lifecycle.initState);

    public static Action build(string name) => new Action(Lifecycle.build, payload: name);

    public static Action reassemble() => new Action(Lifecycle.reassemble);

    public static Action dispose() => new Action(Lifecycle.dispose);

    // static Action didDisposed() => const Action(Lifecycle.didDisposed);

    public static Action didUpdateWidget() => new Action(Lifecycle.didUpdateWidget);

    public static Action didChangeDependencies() => new Action(Lifecycle.didChangeDependencies);

    public static Action deactivate() => new Action(Lifecycle.deactivate);

    public static Action appear(int index) => new Action(Lifecycle.appear, payload: index);

    public static Action disappear(int index) =>
        new Action(Lifecycle.disappear, payload: index);
}

/// [ComponentContext]
/// Definition context of component or page.
public class ComponentContext<T>
{
    private ViewBuilder<T>? view;
    private Dependencies<T>? _dependencies;
    private Effect<T>? effect;
    public Store<object> store;
    private Get<T> getState;
    private MarkNeedsBuild? markNeedsBuild;
    private ShouldUpdate<T> _shouldUpdate;
    Dispatch? _dispatch;
    Dispatch? _effectDispatch;

    public ComponentContext(Store<object> store, Get<T> getState, Dependencies<T>? dependencies = null, MarkNeedsBuild? markNeedsBuild = null, ViewBuilder<T>? view = null, Effect<T>? effect = null, ShouldUpdate<T>? shouldUpdate = null)
    {
        this.store = store;
        this.getState = getState;
        this.markNeedsBuild = markNeedsBuild;
        this._dependencies = dependencies;
        this.view = view;
        this.effect = effect;
        this._shouldUpdate = shouldUpdate ?? _updateByDefault<T>();
    }

    T state => getState();
    Widget? _widgetCache;
    T? _latestState;


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

    Dispatch? dispatch(Action action) => _dispatch?.Invoke(action);
    //void broadcastEffect(Action action, bool? excluded) => _bus
    //    ?.dispatch(action, excluded: excluded == true ? _effectDispatch : null);

    Widget buildComponent(string type)
    {
        Dependent<T>? dependent = _dependencies?.slot(type);
        return dependent!.buildComponent(
          store,
          getState
        );
    }

    List<Widget> buildComponents()
    {
        Dependent<T> dependent = _dependencies!.adapter!;
        return dependent.buildComponents(
          store,
          getState
        );
    }

    //    Function()? _dispatchDispose;

    Dispatch _createNextDispatch(ComponentContext<T> ctx) => (Redux.Action action) =>
    {
        ctx.store.Dispatch?.Invoke(action);
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
        return (Action action) =>
        {
            object? result = userEffect?.Invoke(action, ctx);

            //skip-lifecycle-actions
            if (action.Type is Lifecycle && (result is null or (object)false))
            {
                return new object();
            }

            return result;
        };
    }

    Dispatch _createDispatch(
            Dispatch onEffect, Dispatch next, ComponentContext<T> ctx) =>
        (Action action) =>
        {
            object? result = onEffect.Invoke(action);
            if (result is null or (object)false)
            {
                next(action);
            }

            return result == new object() ? null : result;

        };

    private ShouldUpdate<K> _updateByDefault<K>() =>
                (K? _, K? __) => !EqualityComparer<K>.Default.Equals(_, __);//!identical(_, __);
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

public delegate Task SubEffect<T>(Action action, ComponentContext<T> ctx);

public static class EffectConverter
{
    static readonly object SubEffectReturnNull = new object();

    /// [combineEffects]
    /// for action.type which override it's == operator
    /// return [UserEffect]
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

public delegate void MarkNeedsBuild();

/// [AbstractComponent]
/// Definition of basic component.
public abstract class BasicComponent<T> : ComponentBase<T>
{
    public Dependencies<T>? _dependencies;
    private Reducer<T> _reducer;
    private Effect<T>? _effect;
    private ViewBuilder<T>? _view;
    private ShouldUpdate<T>? _shouldUpdate;

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
        return ReducerConverter.CombineReducers<T>(new List<Reducer<T>> { _reducer, _dependencies?.createReducer() ?? ((T state, Action action) => state) })
            ?? ((T state, Action action) => state);
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
    public ComposedComponent(
       Reducer<T> reducer, ShouldUpdate<T>? shouldUpdate = null)
      : base(reducer: reducer, view: null, shouldUpdate: shouldUpdate) { }

    public override Widget buildComponent(Store<object> store, Get<T> getter)
    {
        throw new NotImplementedException("omposedComponent could not build single component");
    }
}

/// [Dependents]
/// Definition of Dependents type, a list of dependent.
//typedef Dependents<T> = List<Dependent<T>>;

/// [BasicAdapter]
/// Definition of basic adapter, it is composed component.
/// only building a list widgets.
public class BasicAdapter<T> : ComposedComponent<T>
{
    ComponentContext<T>? _ctx;
    Func<T, List<Dependent<T>>> builder = (_) => new List<Dependent<T>>();
    public BasicAdapter(
        Reducer<T>? reducer,
        Func<T, List<Dependent<T>>> builder,
        ShouldUpdate<T>? shouldUpdate
      ) : base(
            reducer: reducer ?? ((T state, Action _) => state),
            shouldUpdate: shouldUpdate
          )
    { }

    Reducer<T> _createAdapterReducer() => (T state, Action action) =>
    {
        T copy = state;
        bool hasChanged = false;
        List<Dependent<T>> list = builder.Invoke(state);
        for (int i = 0; i < list.Count; i++)
        {
            Dependent<T> dep = list[i];
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
        return ReducerConverter.CombineReducers<T>(new List<Reducer<T>>
        {
            base.createReducer(), _createAdapterReducer()
        }) ?? ((T state, Action action) => state);
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
        List<Widget> widgets = new List<Widget>();
        for (int i = 0; i < dependentArray.Count; i++)
        {
            Dependent<T> dependent = dependentArray[i];
            widgets.AddRange(
              dependent.buildComponents(
                store,
                getter)
            );
        }
        _ctx!.onLifecycle(LifecycleCreator.initState());
        return widgets;
    }
}

public static class ObjectCopier
{
    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static T? Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", nameof(source));
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default;
        }

        var stream = System.Text.Json.JsonSerializer.Serialize<T>(source);
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream);
    }
}