// ReSharper disable CheckNamespace
namespace Redux.Component;

/// init store's state by route-params
public delegate T InitState<T, P>(P? param);

public abstract class Page<T, P> : Component<T>
{
    private InitState<T, P> _initState;

    protected Page(InitState<T, P> initState, ViewBuilder<T> view, Effect<T>? effect, Reducer<T>? reducer) : base(view, effect, reducer)
    {
        _initState = initState;
    }
}