namespace Redux.Component;

/// Define a basic behavior of routes.
public interface AbstractRoutes
{
    public string home { get; }
    public Widget buildPage(string? path, dynamic? arguments = null);
}

/// Each page has a unique store.
public class PageRoutes : AbstractRoutes
{
    readonly Map pages;
    readonly string? initialRoute;

    public PageRoutes(Map? pages, string? initialRoute = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(pages));
        this.pages = pages ?? new Map();
        this.initialRoute = initialRoute;
    }

    string? initialRoutePath =>
          initialRoute ?? pages.Keys.FirstOrDefault();

    public string home => initialRoutePath ?? string.Empty;

    public Widget buildPage(string? path, dynamic? arguments = null) => pages[path!].buildPage(arguments);
}
