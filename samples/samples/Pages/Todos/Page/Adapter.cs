using System.Collections.ObjectModel;
using System.Linq;
using samples.Pages.Todos.Report;
using samples.Pages.Todos.Todo;

namespace samples.Pages.Todos.Page;

internal class PageAdapter : BasicAdapter<PageState>
{
    public PageAdapter() : base(builder: dependentBuilder)
    {
    }

    static Dependents<PageState> dependentBuilder => (state) => state.ToDos!
        .Select((todo, index) =>
            new TodoConnector(toDos: state.ToDos!, index: index) + new TodoComponent())
        .ToList();
}

internal class TodoConnector : ConnOp<PageState, ToDoState>
{
    ObservableCollection<ToDoState> toDos;
    int index;

    internal TodoConnector(ObservableCollection<ToDoState> toDos, int index) : base()
    {
        this.toDos = toDos;
        this.index = index;
    }

    public override ToDoState Get(PageState state)
    {
        return toDos[index];
    }

    public override void Set(PageState state, ToDoState subState)
    {
        base.Set(state, subState);
        state.ToDos![index] = subState;
    }
}

internal class ReportConnector : ConnOp<PageState, ReportState>
{
    public override ReportState Get(PageState state)
    {
        return new ReportState(state.ToDos!.Count, state.ToDos!.Count((e) => e.IsDone));
    }

    public override void Set(PageState state, ReportState subState) { }
}

