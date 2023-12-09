using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace samples.Pages.Todos.Report;

internal class ReportState(int total, int done) : ReactiveObject
{
    [Reactive]
    public int Total { get; set; } = total;

    [Reactive]
    public int Done { get; set; } = done;

    public override string ToString()
    {
        return $"ReportState: total: {Total}, done: {Done}";
    }
}
