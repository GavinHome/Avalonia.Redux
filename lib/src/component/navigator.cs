/* 
 * [ToDo]
 * The navigator is not perfect, but I have to a simple implementation for route.
 * 
 */
// ReSharper disable ClassNeverInstantiated.Global
namespace Redux.Component;

/// [RouteSettings]
public record RouteSettings(string name = "", dynamic? arguments = null);

/// [RouteFactory]
/// Definition of a standard RouteFactory function.
public delegate dynamic RouteFactory(RouteSettings settings);

/// [Route]
public class Route<T> where T : class
{
    private readonly RouteSettings _settings;
    private readonly dynamic? _content;

    public dynamic Content => _content!;
    public RouteSettings Settings => _settings;

    public Action<T>? Func { get; internal set; }

    public Route(RouteSettings? settings, dynamic content)
    {
        _settings = settings ?? new RouteSettings();
        _content = content;
    }
}

/// [_RouteEntry]
class _RouteEntry
{
    readonly Route<dynamic>? _route;

    public Route<dynamic> Route => _route!;

    public _RouteEntry(Route<dynamic> route)
    {
        _route = route;
    }
}

/// [Navigator]
public class Navigator : StatefulWidget
{
#pragma warning disable CA2211 // 非常量字段应当不可见
    public static System.Action? onChange;
#pragma warning restore CA2211 // 非常量字段应当不可见
    private static readonly NavigatorState navigatorState = new();

    public static RouteFactory? onGenerateRoute { get; set; }

    public static NavigatorState of(dynamic? _ = null)
    {
        return navigatorState;
    }

    public override NavigatorState createState() => navigatorState;
}

/// [NavigatorState]
public class NavigatorState : State<StatefulWidget>
{
    readonly Stack<_RouteEntry> _history = new();
    private _RouteEntry? _current;

    public Widget current => _current!.Route.Content;

    public override Widget build(dynamic context)
    {
        throw new NotImplementedException();
    }

    public async Task<Route<dynamic>> pushNamed<T>(string routeName, dynamic? arguments, Action<T?>? call = null) where T : class
    {
        Route<dynamic> route = _routeNamed<dynamic>(routeName: routeName, arguments: arguments);
        route.Func = (x) => call?.Invoke((T)Convert.ChangeType(x, typeof(T)));
        return await push(route);
    }

    public async Task<Route<dynamic>> pop<T>(T? result)
    {
        return await _pop(result);
    }

    public Task<Route<dynamic>> push(Route<dynamic> route)
    {
        if (route == null)
        {
            throw new ArgumentNullException(nameof(route));
        }

        _pushEntry(new _RouteEntry(route));
        return Task.Run(() => _history.Pop().Route);
    }

    Route<T> _routeNamed<T>(string routeName, dynamic? arguments) where T : class
    {
        var content = Navigator.onGenerateRoute?.Invoke(new RouteSettings(routeName, arguments));
        Route<T> route = new(new RouteSettings(routeName, arguments), content);
        return route;
    }

    void _pushEntry(_RouteEntry entry)
    {
        if (_current != null && !_current.Equals(entry))
        {
            _history.Push(_current!);
        }

        _current = entry;
        _history.Push(entry);
        Navigator.onChange?.Invoke();
    }

    Task<Route<dynamic>> _pop<T>(T? result)
    {
        _current?.Route.Func?.Invoke(result as dynamic);
        var entry = _history.Pop();
        _current = entry;
        Navigator.onChange?.Invoke();
        return Task.Run(() => _current.Route);
    }
}
