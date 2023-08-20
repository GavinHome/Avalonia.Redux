namespace Redux.Component;

public abstract class Component<T> : AbstractComponent<T>
{
    protected Component(ViewBuilder<T> view, Effect<T> effect, Reducer<T> reducer) : base(view, effect,
        reducer) {}
}