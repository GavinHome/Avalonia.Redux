namespace samples.Pages.Todos.Edit;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using samples.Pages.Todos.Todo;
using samples.Views;

public partial class TodoEditPage() : Page<TodoEditState, ToDoState>(initState: initState,
    effect: buildEffect(),
    middlewares:
    [
        Redux.Middlewares.logMiddleware<TodoEditState>(tag: "TodoEditPage")
    ],
    view: (state, dispatch, ctx) =>
    {
        return new DockPanel
        {
            [Grid.IsSharedSizeScopeProperty] = true,
            Children =
            {
                new StackPanel
                {
                    [DockPanel.DockProperty] = Dock.Bottom,
                    [Visual.ZIndexProperty] = 99,
                    Children =
                    {
                        new Border
                        {
                            Padding = Thickness.Parse("8"),
                            Child = new Grid
                            {
                                Children =
                                {
                                    new Border
                                    {
                                        Margin = Thickness.Parse("0 -20 10 8"),
                                        HorizontalAlignment = HorizontalAlignment.Center,
                                        VerticalAlignment = VerticalAlignment.Center,
                                        Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                        Padding = new Thickness(0),
                                        CornerRadius = new CornerRadius(15),
                                        Child = new Button()
                                        {
                                            Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                            CornerRadius = new CornerRadius(15),
                                            Padding = new Thickness(0),
                                            Height = 50,
                                            Width = 50,
                                            BorderThickness = new Thickness(0),
                                            Content = new Border
                                            {
                                                Padding = new Thickness(8, 5, 12, 8),
                                                Child = new Path
                                                {
                                                    Data = Geometry.Parse(
                                                        "M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z"),
                                                    Fill = new SolidColorBrush(Colors.Black),
                                                    HorizontalAlignment = HorizontalAlignment.Center,
                                                    VerticalAlignment = VerticalAlignment.Center,
                                                },
                                            },
                                            Command = ReactiveCommand.Create(() =>
                                                dispatch(ToDoEditActionCreator.onDone()))
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new StackPanel
                {
                    Children =
                    {
                        new Grid
                        {
                            Margin = Thickness.Parse("8"),
                            Children =
                            {
                                new ScrollViewer
                                {
                                    [!Layoutable.HeightProperty] = new Binding()
                                    {
                                        Path = "Height",
                                        RelativeSource = new RelativeSource()
                                        {
                                            Mode = RelativeSourceMode.FindAncestor,
                                            AncestorType = typeof(MainWindow)
                                        }
                                    },
                                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                    Content = new StackPanel
                                    {
                                        Children =
                                        {
                                            new Border
                                            {
                                                Child = new TextBlock { Text = "Edit Todo" },
                                            },
                                            new Border
                                            {
                                                Padding = Thickness.Parse("20 50"),
                                                Child = new Grid
                                                {
                                                    ColumnDefinitions =
                                                    [
                                                        new ColumnDefinition(GridLength.Auto),
                                                        new ColumnDefinition(GridLength.Star)
                                                    ],
                                                    RowDefinitions =
                                                    [
                                                        new RowDefinition(GridLength.Auto),
                                                        new RowDefinition(GridLength.Auto),
                                                    ],
                                                    Children =
                                                    {
                                                        new Border
                                                        {
                                                            [Grid.RowProperty] = 0,
                                                            [Grid.ColumnProperty] = 0,
                                                            Child = new TextBlock
                                                            {
                                                                Text = "title:",
                                                                HorizontalAlignment = HorizontalAlignment.Right,
                                                                FontSize = 20,
                                                                Foreground = new SolidColorBrush(Colors.Black),
                                                                VerticalAlignment = VerticalAlignment.Stretch
                                                            },
                                                        },
                                                        new Border
                                                        {
                                                            [Grid.RowProperty] = 0,
                                                            [Grid.ColumnProperty] = 1,
                                                            Padding = Thickness.Parse("8 0"),
                                                            Child = new TextBox
                                                            {
                                                                [!TextBlock.TextProperty] = new Binding()
                                                                {
                                                                    Source = state.toDo,
                                                                    Path = "Title",
                                                                },
                                                                FontSize = 16,
                                                                Foreground = new SolidColorBrush(Colors.Black)
                                                            },
                                                        },
                                                        new Border
                                                        {
                                                            [Grid.RowProperty] = 1,
                                                            [Grid.ColumnProperty] = 0,
                                                            Padding = Thickness.Parse("0 32"),
                                                            Child = new TextBlock
                                                            {
                                                                Text = "desc:",
                                                                HorizontalAlignment = HorizontalAlignment.Right,
                                                                FontSize = 20,
                                                                Foreground = new SolidColorBrush(Colors.Black),
                                                                VerticalAlignment = VerticalAlignment.Stretch
                                                            },
                                                        },
                                                        new Border
                                                        {
                                                            [Grid.RowProperty] = 1,
                                                            [Grid.ColumnProperty] = 1,
                                                            Padding = Thickness.Parse("8 32"),
                                                            Child = new TextBox
                                                            {
                                                                [!TextBlock.TextProperty] = new Binding()
                                                                {
                                                                    Source = state.toDo,
                                                                    Path = "Desc",
                                                                },
                                                                Height = 200,
                                                                AcceptsReturn = true,
                                                                TextWrapping = TextWrapping.Wrap,
                                                                MaxLines = 10,
                                                                FontSize = 16,
                                                                Foreground = new SolidColorBrush(Colors.Black)
                                                            },
                                                        },
                                                    }
                                                },
                                            },
                                        },
                                    }
                                }
                            }
                        },
                    }
                }
            }
        };
    })
{
    private static TodoEditState initState(ToDoState? arg) => new() { toDo = arg?.Clone() ?? new() };
}
