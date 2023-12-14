using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace samples.Pages.Todos.Report;

public class ReportState : ReactiveObject
{
    [Reactive]
    public int Total { get; set; }

    [Reactive]
    public int Done { get; set; }

    public ReportState(int total, int done)
    {
        Total = total;
        Done = done;
    }

    public ReportState() { }

    public override string ToString()
    {
        return $"ReportState: total: {Total}, done: {Done}";
    }
}
