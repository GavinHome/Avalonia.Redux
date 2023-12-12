namespace Redux;

/// Middleware for print action dispatch.
/// It works on debug mode.
public static class Middlewares
{
    public static Middleware<T> logMiddleware<T>(Func<T, String>? monitor = null, String tag = "avalonia-redux")
    {
        return (_, getState) =>
            (next) =>
            {
                Action<Object> print = (obj) => System.Diagnostics.Trace.WriteLine($"[AvaloniaRedux]: {obj}");

                Dispatch log = (Action action) =>
                {
                    print($"---------- [{tag}] ----------");
                    print($"[{tag}] {action.Type.GetType().Name}.{action.Type} {action.Payload as object}");

                    T prevState = getState();
                    if (monitor != null)
                    {
                        print($"[{tag}] prev-state: {monitor(prevState)}");
                    }

                    next(action);

                    T nextState = getState();
                    if (monitor != null)
                    {
                        print($"[{tag}] next-state: {monitor(nextState)}");
                    }

                    print($"========== [{tag}] ================");
                    return null;
                };

                return Aop.isDebug() ? log : next;
            };
    }
}

public static class Aop
{
    private static bool isTest { get; set; }

    /// Is app run a debug mode.
    public static bool isDebug()
    {
        return isTest || System.Diagnostics.Debugger.IsAttached;
    }

    public static void setTest()
    {
        isTest = true;
    }
}