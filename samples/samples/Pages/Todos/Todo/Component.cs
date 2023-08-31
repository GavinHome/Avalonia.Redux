using Avalonia;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;

namespace samples.Pages.Todos.Todo;

using Avalonia.Controls;
using Action = Redux.Action;

internal partial class TodoComponent : Component<ToDoState>
{
    public TodoComponent() : base(
        effect: buildEffect(),
        reducer: buildReducer(),
        view: (state, dispatch, _) => new ContentControl
        {
            Padding = Thickness.Parse("10 10"),
            Content = new Grid
            {
                Background = new SolidColorBrush(state.IsDone? Colors.Green: Colors.Red),
                RowDefinitions = new RowDefinitions
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto)
                },
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto)
                },
                Children =
                {
                    new TextBlock
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 18,
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 0,
                        [Grid.ColumnProperty] = 0,
                        [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.Title) }
                    },
                    new TextBlock
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 1,
                        [Grid.ColumnProperty] = 0,
                        [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.Desc) }
                    },
                    new TextBlock
                    {
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 2,
                        [Grid.ColumnProperty] = 0,
                        [!TextBlock.TextProperty] = new Binding { Source = state, Path = nameof(state.IsDone) }
                    },
                    new Button
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 1,
                        [Grid.ColumnProperty] = 1,
                        Content = "edit",
                        Command = ReactiveCommand.Create(() => dispatch(ToDoActionCreator.onEditAction(state.UniqueId)))
                    },
                    !state.IsDone? new Button
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 1,
                        [Grid.ColumnProperty] = 2,
                        Content = "done",
                        Command = ReactiveCommand.Create(() => dispatch(ToDoActionCreator.doneAction(state.UniqueId)))
                    } : new ContentControl(),
                    new Button
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 1,
                        [Grid.ColumnProperty] = 3,
                        Content = "delete",
                        Command = ReactiveCommand.Create(() => dispatch(ToDoActionCreator.onRemoveAction(state.UniqueId)))
                    }
                }
            }
        })
    {
    }
}