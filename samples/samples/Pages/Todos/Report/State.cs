using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace samples.Pages.Todos.Report;

internal class ReportState : ReactiveObject
{
    [Reactive]
    public int Total { get; init; }

    [Reactive]
    public int Done { get; init; }

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
