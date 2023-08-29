using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;

namespace samples.Pages.Counter;

public partial class CounterPage : Page<CounterState, Dictionary<string, dynamic>>
{
    public CounterPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        middlewares: new[]
        {
            Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterPage")
        },
        view: (state, dispatch, _) =>
        {
            return new WidgetWrapper
            {
                Content = new Grid
                {
                    Margin = Thickness.Parse("10"),
                    RowDefinitions = new RowDefinitions
                    {
                        new() { Height = GridLength.Star },
                        new() { Height = GridLength.Auto }
                    },
                    Children =
                    {
                        new StackPanel
                        {
                            [Grid.RowProperty] = 0,
                            Orientation = Orientation.Vertical,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Children =
                            {
                                new TextBlock
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Text = "You have pushed the button this many times:"
                                },
                                new TextBlock
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    [!TextBlock.TextProperty] = new Binding
                                        { Source = state, Path = nameof(state.Count) }
                                }
                            }
                        },
                        new Button()
                        {
                            [Grid.RowProperty] = 1,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Center,
                            Content = new Panel
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Height = 48,
                                Width = 48,
                                Children =
                                {
                                    // new Image
                                    // {
                                    //     Source =  new Bitmap(AssetLoader.Open(new Uri("avares://samples/Assets/avalonia-logo.ico"))),
                                    //     HorizontalAlignment = HorizontalAlignment.Stretch,
                                    //     VerticalAlignment = VerticalAlignment.Bottom,
                                    //     Width = 70,
                                    // }
                                    new Path
                                    {
                                        Data = Geometry.Parse("M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z"),
                                        Fill = new SolidColorBrush(Colors.Pink),
                                        HorizontalAlignment = HorizontalAlignment.Center,
                                        VerticalAlignment = VerticalAlignment.Center,
                                    }
                                }
                            },
                            Command = ReactiveCommand.Create(() => dispatch(CounterActionCreator.onAddAction()))
                        }
                    }
                }
            };
        })
    { }

    private static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 0 };
}
