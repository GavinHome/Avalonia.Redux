using Avalonia.Controls;
using Avalonia.Data;

namespace samples.Pages.Todos.Report;

internal class ReportComponent : Component<ReportState>
{
    public ReportComponent() : base(
        view: (state, dispatch, _) => new StackPanel
        {
            Children =
            {
                new TextBlock
                {
                    // [!TextBlock.TextProperty] = new Subject<ReportState>().
                    //    Select(x => $"Total {state.Total} tasks, {state.Done} done.").ToBinding<string>(),
                    [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.Total) }
                },
                new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.Done) }
                }
            }
        })
    { }
}