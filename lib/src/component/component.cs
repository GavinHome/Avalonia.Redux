// ReSharper disable CheckNamespace
namespace Redux.Component;

public abstract class Component<T> : BasicComponent<T>
{
    protected Component(ViewBuilder<T>? view, Effect<T>? effect = null, Reducer<T>? reducer = null, Dependencies<T>? dependencies = null, ShouldUpdate<T>? shouldUpdate = null)
        : base(view: view,
               effect: effect,
               reducer: reducer ?? ((T state, Action action) => state),
               dependencies: dependencies,
               shouldUpdate: shouldUpdate)
    { }

    public override Widget buildComponent(Store<object> store, Get<T> getter)
    {
        return new _ComponentWidget<T>(component: this, store: store, getter: getter, dependencies: _dependencies).create().build();
    }

    public override List<Widget> buildComponents(Store<object> store, Get<T> getter)
    {
        return new List<Widget> { buildComponent(store, getter) };
    }
}

public class _ComponentWidget<T> : StatefulWidget
{
    BasicComponent<T> component;
    Store<object> store;
    Get<T> getter;
    Dependencies<T>? dependencies;

    public _ComponentWidget(BasicComponent<T> component, Store<object> store, Get<T> getter, Dependencies<T>? dependencies)
    {
        this.component = component;
        this.store = store;
        this.getter = getter;
        this.dependencies = dependencies;
    }

    ////public override State<StatefulWidget> createState()
    ////{
    ////    return (new _ComponentState<_ComponentWidget<T>>() as State<StatefulWidget>)!;
    ////}

    public override State createState() => new _ComponentState<T>();

    public BasicComponent<T> Component => component;
    public Store<object> Store => store;
    public Get<T> GetGetter => getter;
}

public class _ComponentState<T> : State //: State<_ComponentWidget<T>>
{
    ComponentContext<T>? _ctx;
    BasicComponent<T> component => widget.Component;
    Unsubscribe? subscribe;

    new _ComponentWidget<T> widget => (_widget as _ComponentWidget<T>)!;

    public override void initState()
    {
        base.initState();
        _ctx = component.createContext(
            widget.Store,
            widget.GetGetter,
            markNeedsBuild: () =>
            {
                if (mounted)
                {
                    setState(() => null);
                }

                Log.doPrint($"{component.GetType()} do reload");
            }
        );
        _ctx.onLifecycle(LifecycleCreator.initState());
        subscribe = _ctx.store.Subscribe?.Invoke(_ctx.onNotify);
    }

    public override Widget build(dynamic context) => _ctx!.buildView();

    public override void didChangeDependencies()
    {
        base.didChangeDependencies();
        _ctx!.onLifecycle(LifecycleCreator.didChangeDependencies());
    }

    public override void deactivate()
    {
        base.deactivate();
        _ctx!.onLifecycle(LifecycleCreator.deactivate());
    }

    public override void reassemble()
    {
        base.reassemble();
        _ctx!.clearCache();
        _ctx.onLifecycle(LifecycleCreator.reassemble());
    }

    public override void didUpdateWidget(StatefulWidget oldWidget)
    {
        base.didUpdateWidget(oldWidget);
        _ctx!.didUpdateWidget();
        _ctx.onLifecycle(LifecycleCreator.didUpdateWidget());
    }

    public override void disposeCtx()
    {
        base.disposeCtx();
        _ctx!.onLifecycle(LifecycleCreator.dispose());
        _ctx!.dispose();
    }

    public override void dispose()
    {
        subscribe?.Cancel();
        base.dispose();
    }
}