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

/// [NavigateFactory]
/// Definition of a standard NavigateFactory function.
public delegate dynamic RouteChanged(Route<dynamic>? route);

/// [Navigator]
public class Navigator : StatefulWidget
{
    private static readonly NavigatorState navigatorState = new();
    public static RouteChanged? onRouteChanged { get; set; }
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

    public NavigatorState() { }

    public override Widget build(dynamic context)
    {
        throw new NotImplementedException();
    }

    public async Task<Route<dynamic>> push<T>(string routeName, dynamic? arguments = null, Action<T?>? call = null) where T : class
    {
        Action<dynamic?>? func = (x) => call?.Invoke((T?)Convert.ChangeType(x, typeof(T)));
        Route<dynamic> route = _routeNamed<dynamic>(routeName: routeName, arguments: arguments, func: func);
        return await push(route);
    }

    public async Task<Route<dynamic>?> pop<T>(T? result)
    {
        return await _pop(result);
    }

    public Task<Route<dynamic>> push(Route<dynamic> route)
    {
        ArgumentNullException.ThrowIfNull(nameof(route));
        _RouteEntry _entry = new(route);
        _history.Push(_entry);
        _current = _history.First();
        Navigator.onRouteChanged?.Invoke(_current.Route);
        return Task.Run(() => _current.Route);
    }

    Route<T> _routeNamed<T>(string routeName, dynamic? arguments = null, Action<dynamic?>? func = null) where T : class
    {
        var content = Navigator.onGenerateRoute?.Invoke(new RouteSettings(routeName, arguments));
        Route<T> route = new(new RouteSettings(routeName, arguments), content, func);
        return route;
    }

    Task<Route<dynamic>?> _pop<T>(T? result)
    {
        _current?.Route.Func?.Invoke(result as dynamic);
        _history.Pop();
        _current = _history.FirstOrDefault();
        Navigator.onRouteChanged?.Invoke(_current?.Route);
        return Task.Run(() => _current?.Route);
    }
}

/// [Route]
public class Route<T> where T : class
{
    private readonly RouteSettings _settings;
    private readonly dynamic? _content;
    private readonly Action<T?>? _func;

    public Route(RouteSettings settings, dynamic content, Action<dynamic?>? func = null)
    {
        _settings = settings;
        _content = content;
        _func = func;
    }

    public RouteSettings Settings => _settings;

    public dynamic Content => _content!;

    public Action<T?>? Func => _func;
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
