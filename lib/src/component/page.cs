// ReSharper disable CheckNamespace
namespace Redux.Component;

/// init store's state by route-params
public delegate T InitState<T, P>(P? param);

public abstract class Page<T, P> : Component<T>
{
    private InitState<T, P> _initState;

    protected Page(InitState<T, P> initState, ViewBuilder<T> view, Effect<T>? effect = null, Reducer<T>? reducer = null, Dependencies<T>? dependencies = null, ShouldUpdate<T>? shouldUpdate = null) : base(
        effect: effect,
        dependencies: dependencies,
        reducer: reducer,
        view: view,
        shouldUpdate: shouldUpdate)
    {
        _initState = initState;
    }
}