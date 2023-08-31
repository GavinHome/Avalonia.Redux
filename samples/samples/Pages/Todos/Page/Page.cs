using Avalonia.Controls;
using samples.Pages.Todos.Report;
using samples.Pages.Todos.Todo;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using samples.Pages.Counter;
using EffectConverter = Redux.Component.EffectConverter;

namespace samples.Pages.Todos.Page;
using Action = Redux.Action;

public partial class ToDoListPage : Page<PageState, Dictionary<string, dynamic>>
{
    public ToDoListPage() : base(
        initState: initState,
        effect: EffectConverter.CombineEffects(new Dictionary<object, SubEffect<PageState>>
        {
            { Lifecycle.initState, _onInit },
            { "onAdd", _onAdd }
        }),
        reducer: ReducerConverter.asReducers(new Dictionary<object, Reducer<PageState>>
        {
            { "initToDos", _init },
            { "add", _add },
            { ToDoAction.remove, _remove }
        }),
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
            var todos = ctx.buildComponents();
            var report = ctx.buildComponent("report");
            return new StackPanel
            {
                Children =
                {
                    new Grid
                    {
                        Margin = Thickness.Parse("8"),
                        RowDefinitions = new RowDefinitions
                        {
                            new() { Height = GridLength.Star },
                            new() { Height = GridLength.Auto }
                        },
                        Children =
                        {
                            new ItemsControl
                            {
                                ItemsSource = todos,
                            },
                        }
                    },
                    new Border
                    {
                        Child = new StackPanel
                        {
                            Children =
                            {
                                new Border
                                {
                                    [Grid.RowProperty] = 1,
                                    Child = report
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
                                        Command = ReactiveCommand.Create(() =>
                                            dispatch(CounterActionCreator.onAddAction()))
                                    }
                                }
                            }
                        }
                    }
                }
            };
        })
    {
    }

    private static PageState initState(Dictionary<string, dynamic>? param) => new PageState() { ToDos = new() };

    private static async Task _onInit(Action action, ComponentContext<PageState> ctx)
    {
        List<ToDoState> initToDos = new List<ToDoState>()
        {
            new ToDoState(uniqueId: "0", title: "Hello world", desc: "Learn how to program.", isDone: true),
            new ToDoState(uniqueId: "1", title: "Hello Avalonia", desc: "Learn how to build an avalonia app.",
                isDone: true),
            new ToDoState(uniqueId: "2", title: "Hello Avalonia Redux",
                desc: "Learn how to use Avalonia Redux in an avalonia app."),
            // new ToDoState(uniqueId: "3", title: "Hello world", desc: "Learn how to program.", isDone: true),
            // new ToDoState(uniqueId: "4", title: "Hello Avalonia", desc: "Learn how to build an avalonia app.",
            //     isDone: true),
            // new ToDoState(uniqueId: "5", title: "Hello Avalonia Redux",
            //     desc: "Learn how to use Avalonia Redux in an avalonia app."),
            // new ToDoState(uniqueId: "6", title: "Hello Avalonia Redux",
            //     desc: "Learn how to use Avalonia Redux in an avalonia app.")
        };

        ctx.Dispatch(new Action("initToDos", payload: initToDos));
        await Task.CompletedTask;
    }

    private static async Task _onAdd(Action action, ComponentContext<PageState> ctx)
    {
        ctx.Dispatch(new Action("add", payload: new ToDoState
        {
            Title = $"Task{new Random().Next(65535)}",
            Desc = $"Task{new Random().Next(65535)}Description",
        }));
        await Task.CompletedTask;
    }

    private static PageState _init(PageState state, Action action)
    {
        List<ToDoState> toDos = action.Payload ?? new List<ToDoState>();
        state.ToDos = toDos;
        return state;
    }

    private static PageState _add(PageState state, Action action)
    {
        ToDoState? toDo = action.Payload;
        if (toDo != null)
        {
            state.ToDos!.Add(toDo);
        }

        return state;
    }

    private static PageState _remove(PageState state, Action action)
    {
        string? unique = action.Payload;
        var item = state.ToDos?.SingleOrDefault(todo => todo.UniqueId == unique);
        if (item != null) state.ToDos?.Remove(item);
        return state;
    }
}
