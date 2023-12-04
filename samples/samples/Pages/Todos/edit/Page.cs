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

public partial class TodoEditPage : Page<TodoEditState, ToDoState>
{
    public TodoEditPage() : base(
        initState: initState,
        effect: buildEffect(),
        middlewares:
        [
            Redux.Middlewares.logMiddleware<TodoEditState>(tag: "TodoEditPage")
        ],
        view: (state, dispatch, ctx) =>
        {
            //new StackPanel
            //{
            //    Children =
            //    {
            //        new TextBlock { Text = "TodoEditPage" },
            //       new Button()
            //                        {
            //                            [Grid.ColumnProperty] = 1,
            //                            Background = new SolidColorBrush(Colors.Transparent),
            //                            CornerRadius = new CornerRadius(3),
            //                            Padding = new Thickness(0),
            //                            BorderThickness = new Thickness(0),
            //                            Content = new Border
            //                            {
            //                                Background = new SolidColorBrush(Colors.Transparent),
            //                                Padding = new Thickness(8, 5, 12, 8),
            //                                Child = new Path
            //                                {
            //                                    Data = Geometry.Parse(
            //                                        "M20.71,7.04C21.1,6.65 21.1,6 20.71,5.63L18.37,3.29C18,2.9 17.35,2.9 16.96,3.29L15.12,5.12L18.87,8.87M3,17.25V21H6.75L17.81,9.93L14.06,6.18L3,17.25Z"),
            //                                    Fill = new SolidColorBrush(Colors.Black),
            //                                    HorizontalAlignment = HorizontalAlignment.Center,
            //                                    VerticalAlignment = VerticalAlignment.Center,
            //                                },
            //                            },
            //                            Command = ReactiveCommand.Create(() =>
            //                                dispatch(ToDoEditActionCreator.onDone()))
            //       }
            //    }
            //}
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
                                                Command = ReactiveCommand.Create(() => dispatch(ToDoEditActionCreator.onDone()))
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
                                                    Child = new TextBlock { Text = "TodoEdit" },
                                                },
                                                new Border
                                                {
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
                                                                Child = new TextBlock { Text = "title: " },
                                                            },
                                                            new Border
                                                            {
                                                                [Grid.RowProperty] = 0,
                                                                [Grid.ColumnProperty] = 1,
                                                                Child = new TextBox {  },
                                                            },
                                                            new Border
                                                            {
                                                                [Grid.RowProperty] = 1,
                                                                [Grid.ColumnProperty] = 0,
                                                                Child = new TextBlock { Text = "desc:" },
                                                            },
                                                            new Border
                                                            {
                                                                [Grid.RowProperty] = 1,
                                                                [Grid.ColumnProperty] = 1,
                                                                Child = new TextBox { Height= 100,AcceptsReturn= true,TextWrapping= TextWrapping.Wrap },
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
    { }

    private static TodoEditState initState(ToDoState? arg) => new() { toDo = new() };
}
