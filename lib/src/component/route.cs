namespace Redux.Component;

using Map = Dictionary<string, dynamic>;
using Widget = Avalonia.Controls.Control;

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
        Navigator.of().push(new Route<dynamic>(new RouteSettings(path, arguments), content, null));
        return content;
    }
}
