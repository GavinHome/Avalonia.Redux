namespace Redux.Component;

public abstract class ComponentBase<T>
{
}

public abstract class ComponentContext<T>
{
}

/// Component's view part
/// 1.State is used to decide how to render
/// 2.Dispatch is used to send actions
/// 3.ComponentContext is used to build sub-components or adapter.
public delegate dynamic ViewBuilder<T>(T state, Dispatch dispatch, ComponentContext<T> ctx); 

/// Interrupt if not null not false
/// bool for sync-functions, interrupted if true
/// Future<void> for async-functions, should always be interrupted.
public delegate dynamic Effect<T>(Action action, ComponentContext<T> ctx);

public delegate Task SubEffect<T>(Action action, ComponentContext<T> ctx);

public static class EffectConvert 
{
    static readonly object SubEffectReturnNull = new object();

    public static Effect<T>? CombineEffects<T>(Dictionary<object, SubEffect<T>>? map) => map == null || !map.Any()
        ? null : (Action action, ComponentContext<T> ctx) =>
        {
            SubEffect<T> subEffect = map.FirstOrDefault(entry => action.Type.Equals(entry.Key)).Value;
            if (subEffect != null)
            {
                return subEffect.Invoke(action, ctx) ?? SubEffectReturnNull;
            }
            
            ////no subEffect
            return null;
        };
}

public abstract class AbstractComponent<T> : ComponentBase<T>
{
}
