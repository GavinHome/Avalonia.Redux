using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using ReactiveUI;
using samples.ViewModels;

namespace samples.Counter;

public partial class CounterPage : Page<CounterState, Dictionary<string, dynamic>>
{
    public static CounterState state { get; private set; } = new CounterState() { Count = 1 };

    //private MainViewModel vm = new MainViewModel();

    public CounterPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducerNew(state),
        middlewares: new Middleware<CounterState>[]
        {
            Redux.Middlewares.logMiddleware<CounterState>(monitor: (state) => state.ToString(), tag: "CounterTag")
        },
        view: (state, dispatch, ctx) =>
        {
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
                        ////new TextBlock
                        ////{
                        ////    Text = state.Count.ToString(),
                        ////},
                        GetCounterControl(),
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

    private static Reducer<CounterState>? buildReducerNew(CounterState vm)
    {
        return ReducerConverter.asReducers(new Dictionary<object, Reducer<CounterState>>
        {
            {
                CounterAction.add, (CounterState state, Redux.Action action) =>
                {
                    CounterState newState = state.Clone(); //clone
                    newState.Count += action.Payload;
                    vm.Count = newState.Count;
                    return newState;
                }
            }
        });
    }

    static TextBlock GetCounterControl()
    {
        var textBlock = new TextBlock();

        var binding = new Binding
        {
            Source = state,
            Path = nameof(state.Count)
        };

        textBlock.Bind(TextBlock.TextProperty, binding);
        return textBlock;
    }

    internal static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 1 };
}
