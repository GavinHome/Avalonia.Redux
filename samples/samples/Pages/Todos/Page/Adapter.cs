using samples.Pages.Todos.Report;
using samples.Pages.Todos.Todo;

namespace samples.Pages.Todos.Page;

internal class PageAdapter() : BasicAdapter<PageState>(builder: dependentBuilder)
{
    static Dependents<PageState> dependentBuilder => (state) => state.ToDos!
        .Select((todo, index) =>
            new TodoConnector(toDos: state.ToDos?.ToList()!, index: index) + new TodoComponent())
        .ToList();
}

internal class TodoConnector : ConnOp<PageState, ToDoState>
{
    readonly List<ToDoState> toDos;
    readonly int index;

    internal TodoConnector(List<ToDoState> toDos, int index) 
    {
        this.toDos = toDos;
        this.index = index;
    }

    public override ToDoState Get(PageState state)
    {
        return toDos[index];
    }

    protected override void Set(PageState state, ToDoState subState)
    {
        base.Set(state, subState);
        state.ToDos![index] = subState;
    }
}

internal class ReportConnector : ConnOp<PageState, ReportState>
{
    private readonly ReportState report = new (0, 0);
    
    public override ReportState Get(PageState state)
    {
        report.Total = state.ToDos!.Count;
        report.Done = state.ToDos!.Count((e) => e.IsDone);
        return report;
    }

    protected override void Set(PageState state, ReportState subState) { }
}

