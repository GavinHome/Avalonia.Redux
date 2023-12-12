using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using samples;
using System.Runtime.Versioning;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
#pragma warning disable IDE0060 // 删除未使用的参数
    private static async Task Main(string[] args) => await BuildAvaloniaApp()
            .WithInterFont()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");
#pragma warning restore IDE0060 // 删除未使用的参数

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}