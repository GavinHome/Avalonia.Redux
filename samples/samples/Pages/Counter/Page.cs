using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;

namespace samples.Counter;

public partial class CounterPage : Page<CounterState, Dictionary<string, dynamic>>
{
    public CounterPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        middlewares: new Middleware<CounterState>[]
        {
            Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterPage")
        },
        view: (state, dispatch, ctx) =>
        {
            return new WidgetWrapper
            {
                Content = new Border
                {
                    Child = new StackPanel
                    {
                        Orientation = Avalonia.Layout.Orientation.Vertical,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "You have pushed the button this many times:"
                            },
                            new TextBlock
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.Count) }
                            },
                            new Button
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                Content = new Panel
                                {
                                     Children =
                                    {
                                        new Image
                                        {
                                            Source =  new Bitmap(AssetLoader.Open(new Uri("avares://samples/Assets/avalonia-logo.ico"))),
                                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                                            Width = 100,
                                        }
                                    }
                                },
                                Command = ReactiveCommand.Create(() => dispatch(CounterActionCreator.onAddAction()))
                            }
                        }
                    }
                }
            };
        })
    { }

    internal static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 0 };
}
