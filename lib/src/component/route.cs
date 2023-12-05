namespace Redux.Component;

using Widget = Avalonia.Controls.Control;
//using Map = Dictionary<string, Page<object, dynamic>>;
using Map = Dictionary<string, dynamic>;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;


/// [Subscribe]
/// Definition of a standard subscription function.
/// input a subscriber and output an unsubscription function.
public delegate dynamic RouteFactory(RouteSettings settings);

/// Define a basic behavior of routes.
public interface AbstractRoutes
{
    Widget home { get; }

    public Widget buildPage(string? path, dynamic arguments);
}

/// Each page has a unique store.
public class PageRoutes : AbstractRoutes
{
    readonly Map pages;
    readonly string? initialRoute;

    public PageRoutes(Map? pages, string? initialRoute = null)
    {
        this.pages = pages ?? new Map();
        this.initialRoute = initialRoute;
    }

    string? initialRoutePath =>
          initialRoute ?? pages.Keys.FirstOrDefault();

    public Widget home => buildHome(initialRoutePath, new Map());

    public Widget buildPage(string? path, dynamic arguments) => pages[path!].buildPage(arguments);

    private Widget buildHome(string? path, dynamic arguments)
    {
        var content = pages[path!].buildPage(arguments);
        Navigator.of().push<Route<dynamic>>(new Route<dynamic>(new RouteSettings(path, arguments), content, null));
        return content;
    }
}

public record RouteSettings(string name = "", dynamic? arguments = null);

public class Route<T> where T : class
{
    RouteSettings _settings;
    dynamic? _content;
    //Action<T?>? _call;

    public dynamic Content => _content!;

    public Action<dynamic>? Func { get; internal set; }

    //public Action<T?>? Func => _call!;

    public Route(RouteSettings? settings, dynamic content, Action<T?>? call= null)
    {
        _settings = settings ?? new RouteSettings();
        _content = content;
        //_call = call;
    }
}

class _RouteEntry
{
    readonly Route<dynamic>? _route;

    public Route<dynamic> Route => _route!;

    public _RouteEntry(Route<dynamic> route)
    {
        _route = route;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Navigator : StatefulWidget
{
    public static Action? onChange;
    private static NavigatorState navigatorState = new NavigatorState();

    public static RouteFactory? onGenerateRoute { get; set; }

    public static NavigatorState of(dynamic? ctx = null)
    {
        return navigatorState;
    }

    public override State createState() => navigatorState;
}

public class NavigatorState : State
{
    readonly Stack<_RouteEntry> _history = new();
    private _RouteEntry? _current;

    public Widget current => _current!.Route.Content;

    public override Widget build(dynamic context)
    {
        throw new NotImplementedException();
    }

    ////public async Task<Route<dynamic>> pushNamed(string routeName, dynamic? arguments)
    ////{
    ////    return await push<Route<dynamic>>(_routeNamed<dynamic>(routeName: routeName, arguments: arguments, null));
    ////}

    public async Task<Route<dynamic>> pushNamed(string routeName, dynamic? arguments, Action<dynamic>? call = null)
    {
        Route<dynamic> route = _routeNamed<dynamic>(routeName: routeName, arguments: arguments);
        route.Func = call;
        return await push<Route<dynamic>>(route);
    }

    public Task<Route<dynamic>> push<T>(Route<dynamic>? route)
    {
        if (route == null)
        {
            throw new ArgumentNullException(nameof(route));
        }

        _pushEntry(new _RouteEntry(route));
        return Task.Run(() => _history.Pop().Route);
    }

    public async Task<Route<dynamic>> pop<T>(T? result)
    {
        return await _pop(result);
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

    Route<dynamic>? _routeNamed<T>(string routeName, T? arguments)
    {
        var content = Navigator.onGenerateRoute?.Invoke(new RouteSettings(routeName, arguments));
        Route<dynamic>? route = new Route<dynamic>(new RouteSettings(routeName, arguments), content);
        return route;
    }
}

////public static class TaskExtensions
////{
////    ////public static async Task<TV> then<T, TV>(this Task<T> task, Func<T, TV> call)
////    ////{
////    ////    var result = await task;
////    ////    return call(result);
////    ////}

////    public static async void then<T>(this Task<T> task, Action<T> call)
////    {
////        var result = await task;
////        call(result);
////    }
////}