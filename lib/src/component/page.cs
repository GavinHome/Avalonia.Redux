namespace Redux.Component;

/// init store's state by route-params
public delegate T InitState<T, P>(P? param);

public abstract class Page<T, P> : Component<T> 
{
    private readonly InitState<T, P> _initState;
    private readonly Middleware<T>[]? _middlewares;

    protected Page(InitState<T, P> initState, ViewBuilder<T> view, Effect<T>? effect = null, Reducer<T>? reducer = null,
                    Dependencies<T>? dependencies = null, Middleware<T>[]? middlewares = null,
                    ShouldUpdate<T>? shouldUpdate = null) : base(
        effect: effect,
        dependencies: dependencies,
        reducer: reducer,
        view: view,
        shouldUpdate: shouldUpdate)
    {
        _initState = initState;
        _middlewares = middlewares;
    }

    public InitState<T, P> InitState => _initState;
    public List<Middleware<T>>? Middlewares => _middlewares?.ToList();

    /// build page
    public Widget buildPage(P? param) => new _PageWidget<T, P>(param: param, page: this).create();
}

class _PageWidget<T, P> : StatefulWidget
{
    readonly P? param;
    readonly Page<T, P> page;

    public _PageWidget(P? param, Page<T, P> page)
    {
        this.param = param;
        this.page = page;
    }

    public Page<T, P> Page => page;
    public P? Param => param;

    public override _PageState<T, P> createState() => new();
}

class _PageState<T, P> : State<StatefulWidget>
{
    Store<T>? _store;
    T? state;

    new _PageWidget<T, P> widget => (_widget as _PageWidget<T, P>)!;

    public override void initState()
    {
        base.initState();
        state = widget.Page.InitState(widget.Param);
        _store = StoreCreator.CreateStore(state, widget.Page.createReducer(), middleware: widget.Page.Middlewares);
    }

    protected override void didChangeDependencies()
    {
        base.didChangeDependencies();
    }

    public override Widget build(dynamic context)
    {
        return widget.Page.buildComponent(_store?.ObjectClone()!, _store!.GetState);
    }

    protected override void dispose()
    {
        base.dispose();
    }
}