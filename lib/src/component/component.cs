// ReSharper disable CheckNamespace
namespace Redux.Component;

public abstract class Component<T> : AbstractComponent<T>
{
    protected Component(ViewBuilder<T>? view, Effect<T>? effect = null, Reducer<T>? reducer = null, Dependencies<T>? dependencies = null, ShouldUpdate<T>? shouldUpdate = null)
        : base(view: view,
               effect: effect,
               reducer: reducer ?? ((T state, Action action) => state),
               dependencies: dependencies,
               shouldUpdate: shouldUpdate) { } 

    public override Widget buildComponent(Store<object> store, Get<T> getter)
    {
        throw new NotImplementedException();
    }

    public override List<Widget> buildComponents(Store<object> store, Get<T> getter)
    {
        return new List<Widget> { buildComponent(store, getter) };
    }
}