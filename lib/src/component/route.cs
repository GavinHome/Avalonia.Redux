namespace Redux.Component;

/// Define a basic behavior of routes.
public interface AbstractRoutes
{
    public Widget buildPage(string? path, dynamic? arguments = null);
    public void buildHome(dynamic? arguments = null);
}

/// Each page has a unique store.
public class PageRoutes : AbstractRoutes
{
    readonly Map pages;
    readonly string? initialRoute;

    public PageRoutes(Map? pages, string? initialRoute = null)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(pages));
        this.pages = pages ?? new Map();
        this.initialRoute = initialRoute;
    }

    string? initialRoutePath =>
          initialRoute ?? pages.Keys.FirstOrDefault();

    public Widget buildPage(string? path, dynamic? arguments = null) => pages[path!].buildPage(arguments);

    public void buildHome(dynamic? arguments = null)
    {
        var path = initialRoutePath!;
        Navigator.of().push<dynamic>(path, arguments);
    }
}
