using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using samples.ViewModels;

namespace samples.Counter;

public partial class CounterPage : Page<CounterState, Dictionary<string, dynamic>>
{
    public static CounterState State { get; private set; } = new CounterState();

    public CounterPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        middlewares: new Middleware<CounterState>[]
        {
            Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterTag")
        },
        updateState: (state) => State.Count = state.Count,
        view: (state, dispatch, ctx) =>
        {
            State.Count = state.Count;
            ////TODO: 优化，state的值会被修改掉，所以需要再updateState更新static State；同时在组件中绑定局部state修改将不生效
            return new WidgetWrapper
            {
                Content = new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "You have pushed the button this many times:"
                        },
                        new TextBlock
                        {
                            [!TextBlock.TextProperty] = new Binding { Source = State, Path = nameof(State.Count) }
                        },
                        new Button
                        {
                            Content = "Increment",
                            Command = ReactiveCommand.Create(() => dispatch(CounterActionCreator.onAddAction()))
                        }
                    }
                }
            };
        })
    { }

    internal static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 88 };
}
