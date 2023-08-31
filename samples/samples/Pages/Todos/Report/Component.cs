using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace samples.Pages.Todos.Report;

internal class ReportComponent : Component<ReportState>
{
    public ReportComponent() : base(
        view: (state, dispatch, _) =>
        {
            return new StackPanel
            {
                Children =
                    {
                        new TextBlock
                        {
                             [!TextBlock.TextProperty] = new Subject<ReportState>().
                                Select(x => $"Total {state.Total} tasks, {state.Done} done.").ToBinding<string>()
                        }
                    }
            };

        })
    { }
}