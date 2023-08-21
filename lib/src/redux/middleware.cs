namespace Redux;

/// Middleware for print action dispatch.
/// It works on debug mode.
public class Middlewares
{
    public static Middleware<T> logMiddleware<T>(System.Func<T, String> monitor, String tag = "done-redux")
    {
        return (Dispatch Dispatch, Get<T> getState) =>
            (Dispatch next) =>
            {
                System.Action<Object> print = (Object obj) => Console.WriteLine(obj);

                Dispatch log = (Action action) =>
                {
                    print($"---------- [{tag}] ----------");
                    print($"[{tag}] {action.Type} {action.Payload}");

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
    public static bool isTest { get; private set; }

    /// Is app run a debug mode.
    public static bool isDebug()
    {
        return isTest || System.Diagnostics.Debugger.IsAttached;
    }

    public static void setTest()
    {
        Aop.isTest = true;
    }
}