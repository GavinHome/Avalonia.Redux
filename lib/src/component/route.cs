namespace Redux.Component;

using Widget = Avalonia.Controls.Control;
using Map = Dictionary<string, Page<object, dynamic>>;

/// Define a basic behavior of routes.
public interface AbstractRoutes
{
    Widget home { get; }
    public abstract Widget buildPage(string? path, dynamic arguments);
}

/// Each page has a unique store.
public class PageRoutes : AbstractRoutes
{
    Map pages;
    string? initialRoute;

    public PageRoutes(Map pages, string? initialRoute = null)
    {
        this.pages = pages ?? new Map();
        this.initialRoute = initialRoute;
    }

    string? initialRoutePath =>
          initialRoute ?? pages.Keys.FirstOrDefault<string>();

    public Widget home => buildPage(initialRoutePath, new Map());

    public Widget buildPage(string? path, dynamic arguments) => pages[path!]!.buildPage(arguments);
}