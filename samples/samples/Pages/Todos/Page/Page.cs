using Avalonia.Controls;
using samples.Pages.Todos.Report;
using samples.Pages.Todos.Todo;
using System.Collections.ObjectModel;

namespace samples.Pages.Todos.Page;
using Action = Redux.Action;

internal partial class ToDoListPage : Page<PageState, Dictionary<string, dynamic>>
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
            {  "add", _add },
            {  ToDoAction.remove, _remove }
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
                        // new ItemsControl
                        // {
                        //      //Items = todos,
                        // },
                        report,
                    }
            };
        })
    { }

    private static PageState initState(Dictionary<string, dynamic>? param) => new PageState() { ToDos = new() };

    private static async Task _onInit(Action action, ComponentContext<PageState> ctx)
    {
        List<ToDoState> initToDos = new List<ToDoState>()
        {
            new ToDoState(uniqueId: "0", title: "Hello world", desc: "Learn how to program.", isDone: true),
            new ToDoState(uniqueId: "1", title: "Hello Avalonia", desc: "Learn how to build an avalonia app.", isDone: true),
            new ToDoState(uniqueId: "2", title: "Hello Avalonia Redux", desc: "Learn how to use Avalonia Redux in an avalonia app.")
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
        ObservableCollection<ToDoState> toDos = action.Payload ?? new ObservableCollection<ToDoState>();
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
        //state.ToDos!.RemoveAll((ToDoState state) => state.UniqueId == unique);
        return state;
    }
}
