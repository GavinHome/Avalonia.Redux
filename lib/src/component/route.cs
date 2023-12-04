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
        Navigator.of().push<Route<dynamic>>(new Route<dynamic>(new RouteSettings(path, arguments), content));
        return content;
    }
}

public record RouteSettings(string name = "", dynamic? arguments = null);

public class Route<T> where T : class
{
    RouteSettings _settings;
    dynamic _content;

    public dynamic Content => _content!;

    public Route(RouteSettings? settings, dynamic content)
    {
        _settings = settings ?? new RouteSettings();
        _content = content;
    }
}

class _RouteEntry
{
    Route<dynamic>? _route;

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
    Stack<_RouteEntry> _history = new Stack<_RouteEntry>();
    private _RouteEntry? _current;

    public Widget current => _current!.Route.Content;

    public override Widget build(dynamic context)
    {
        throw new NotImplementedException();
    }

    public Task<Route<dynamic>> pushNamed<T>(string routeName, T arguments)
    {
        return push<Route<dynamic>>(_routeNamed<T>(routeName: routeName, arguments: arguments)!);
    }

    public Task<Route<dynamic>> push<T>(Route<dynamic> route)
    {
        _pushEntry(new _RouteEntry(route));
        return Task.Run(() => _history.Pop().Route);
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

    public Task<Route<dynamic>> pop<T>(dynamic? result)
    {
        var entry = _history.Pop();
        _current = entry;
        Navigator.onChange?.Invoke();
        return Task.Run(() => _current.Route);
    }

    Route<dynamic>? _routeNamed<T>(string routeName, dynamic? arguments)
    {
        var widget = Navigator.onGenerateRoute?.Invoke(new RouteSettings(routeName, arguments));
        Route<dynamic>? route = new Route<dynamic>(new RouteSettings(routeName, arguments), widget);
        return route;
    }
}

public static class TaskExtensions
{
    ////public static async Task<TV> then<T, TV>(this Task<T> task, Func<T, TV> call)
    ////{
    ////    var result = await task;
    ////    return call(result);
    ////}

    public static async void then<T>(this Task<T> task, Action<T> call)
    {
        var result = await task;
        call(result);
    }
}