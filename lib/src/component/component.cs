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
        return new _ComponentWidget<T>(component: this, store: store, getter: getter, dependencies: _dependencies);
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

    //public override _ComponentState<T> createState() => new _ComponentState<T>();
}

public class _ComponentState<T> : State<_ComponentWidget<T>>
{
    ComponentContext<T> _ctx;
    //BasicComponent<T> component => widget.component;

    protected override void initState()
    {
        throw new NotImplementedException();
    }

    //    late Function() subscribe;

    //    @override
    //  void initState()
    //    {
    //        super.initState();
    //        _ctx = component.createContext(
    //        widget.store,
    //          widget.getter,
    //          buildContext: context,
    //          bus: widget.bus,
    //          markNeedsBuild: () {
    //            if (mounted)
    //            {
    //                setState(() { });
    //}
    //Log.doPrint('${component.runtimeType} do reload');
    //      },
    //    );
    //_ctx.onLifecycle(LifecycleCreator.initState());
    //subscribe = _ctx.store.subscribe(_ctx.onNotify);
    //  }

    //  @override
    //  Widget build(BuildContext context) => _ctx.buildView();

    //@mustCallSuper
    //@override
    //  void didChangeDependencies() {
    //    super.didChangeDependencies();
    //    _ctx.onLifecycle(LifecycleCreator.didChangeDependencies());
    //}

    //@mustCallSuper
    //@override
    //  void deactivate() {
    //    super.deactivate();
    //    _ctx.onLifecycle(LifecycleCreator.deactivate());
    //}

    //@override
    //@protected
    //  @mustCallSuper
    //  void reassemble() {
    //    super.reassemble();
    //    _ctx.clearCache();
    //    _ctx.onLifecycle(LifecycleCreator.reassemble());
    //}

    //@mustCallSuper
    //@override
    //  void didUpdateWidget(_ComponentWidget<T> oldWidget) {
    //    super.didUpdateWidget(oldWidget);
    //    _ctx.didUpdateWidget();
    //    _ctx.onLifecycle(LifecycleCreator.didUpdateWidget());
    //}

    //@mustCallSuper
    //  void disposeCtx() {
    //    _ctx..onLifecycle(LifecycleCreator.dispose())..dispose();
    //}

    //@mustCallSuper
    //@override
    //  void dispose() {
    //    disposeCtx();
    //    subscribe();
    //    super.dispose();
    //}
}