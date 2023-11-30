using Avalonia.Controls;
using samples.Pages.Todos.Report;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using samples.Views;
////using DynamicData.Binding;
////using DynamicData;
////using Avalonia.Threading;

namespace samples.Pages.Todos.Page;
using Action = Redux.Action;

public partial class ToDoListPage : Page<PageState, Dictionary<string, dynamic>>
{
    public ToDoListPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        middlewares: new[]
        {
            Redux.Middlewares.logMiddleware<PageState>(monitor: (state) => state.ToString(), tag: "ToDoListPage")
        },
        dependencies: new Dependencies<PageState>(
            adapter: new NoneConn<PageState>() + new PageAdapter(),
            slots: new Dictionary<String, Dependent<PageState>>
            {
                { "report", new ReportConnector() + new ReportComponent() },
            }
        ),
        view: (state, dispatch, ctx) =>
        {
            var toDos = ctx.buildComponents();
            var getToDos = () => ctx.buildComponents();
            var report = ctx.buildComponent("report");

            ////TODO: It works, but not perfect
            ////state.Items?.AddRange(getToDos());
            ////state.ToDos!.CollectionChanged += (e, a) =>
            ////{
            ////    Dispatcher.UIThread.Post(() =>
            ////    {
            ////        state.Items?.Clear();
            ////        state.Items?.AddRange(getToDos());
            ////    });
            ////};

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
                                            Child = report,
                                        },
                                        new Border
                                        {
                                            Margin = Thickness.Parse("0 -20 10 8"),
                                            HorizontalAlignment = HorizontalAlignment.Right,
                                            VerticalAlignment = VerticalAlignment.Center,
                                            Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                            Padding = new Thickness(0),
                                            CornerRadius = new CornerRadius(15),
                                            Child = new Button()
                                            {
                                                Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                                CornerRadius = new CornerRadius(15),
                                                Padding = new Thickness(0),
                                                Height = 50, Width = 50,
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
                                                    dispatch(new Action("onAdd")))
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
                                        Content = new ItemsControl
                                        {
                                            ItemsSource = toDos,
                                            ////[!ItemsControl.ItemsSourceProperty] = new Binding()
                                            ////{
                                            ////    Source = state,
                                            ////    Path = "Items",
                                            ////    FallbackValue = "wait a moment"
                                            ////},
                                        },
                                    }
                                }
                            },
                        }
                    }
                }
            };
        })
    {
    }

    private static PageState initState(Dictionary<string, dynamic>? param) => new PageState() { ToDos = new(), Items = new() };
}
