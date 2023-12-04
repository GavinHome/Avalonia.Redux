namespace samples.Pages.Todos.Edit;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using samples.Pages.Todos.Todo;

public partial class TodoEditPage : Page<TodoEditState, ToDoState>
{
    public TodoEditPage() : base(
        initState: initState,
        effect: buildEffect(),
        middlewares: new[]
        {
            Redux.Middlewares.logMiddleware<TodoEditState>(tag: "TodoEditPage")
        },
        view: (state, dispatch, ctx) => new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "TodoEditPage" },
               new Button()
                                {
                                    [Grid.ColumnProperty] = 1,
                                    Background = new SolidColorBrush(Colors.Transparent),
                                    CornerRadius = new CornerRadius(3),
                                    Padding = new Thickness(0),
                                    BorderThickness = new Thickness(0),
                                    Content = new Border
                                    {
                                        Background = new SolidColorBrush(Colors.Transparent),
                                        Padding = new Thickness(8, 5, 12, 8),
                                        Child = new Path
                                        {
                                            Data = Geometry.Parse(
                                                "M20.71,7.04C21.1,6.65 21.1,6 20.71,5.63L18.37,3.29C18,2.9 17.35,2.9 16.96,3.29L15.12,5.12L18.87,8.87M3,17.25V21H6.75L17.81,9.93L14.06,6.18L3,17.25Z"),
                                            Fill = new SolidColorBrush(Colors.Black),
                                            HorizontalAlignment = HorizontalAlignment.Center,
                                            VerticalAlignment = VerticalAlignment.Center,
                                        },
                                    },
                                    Command = ReactiveCommand.Create(() =>
                                        dispatch(ToDoEditActionCreator.onDone()))
               }
            }
        })
    { }
    
    private static TodoEditState initState(ToDoState? arg) => new TodoEditState() { toDo = new() };
}
