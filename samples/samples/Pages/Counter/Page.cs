using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;

namespace samples.Pages.Counter;

public partial class CounterPage() : Page<CounterState, Dictionary<string, dynamic>>(initState: initState,
    effect: buildEffect(),
    reducer: buildReducer(),
    middlewares:
    [
        Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterPage")
    ],
    view: (state, dispatch, _) =>
    {
        return new ContentControl
        {
            Content = new Grid
            {
                Margin = Thickness.Parse("10"),
                RowDefinitions =
                [
                    new() { Height = GridLength.Star },
                    new() { Height = GridLength.Auto }
                ],
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
                    new Border
                    {
                        [Grid.RowProperty] = 1,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = SolidColorBrush.Parse("#bbe9d3ff"),
                        Padding = new Thickness(0),
                        CornerRadius = new CornerRadius(15),
                        BoxShadow = new BoxShadows(new BoxShadow()
                            { OffsetX = 1, OffsetY = 5, Spread = 1, Blur = 8, Color = Colors.LightGray }),
                        Child = new Button()
                        {
                            Background = new SolidColorBrush(Colors.Transparent),
                            CornerRadius = new CornerRadius(15),
                            Padding = new Thickness(0),
                            Height = 50, Width = 50,
                            BorderThickness = new Thickness(0),
                            Content = new Border
                            {
                                Background = new SolidColorBrush(Colors.Transparent),
                                Padding = new Thickness(8, 5, 12, 8),
                                Child = new Path
                                {
                                    Data = Geometry.Parse("M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z"),
                                    Fill = new SolidColorBrush(Colors.Black),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                },
                            },
                            Command = ReactiveCommand.Create(() => dispatch(CounterActionCreator.onAddAction()))
                        }
                    }
                }
            }
        };
    })
{
    private static CounterState initState(Dictionary<string, dynamic>? param) => new() { Count = 99 };
}
