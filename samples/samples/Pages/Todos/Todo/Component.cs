using System.Collections;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Converters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Converters;
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
            Padding = Thickness.Parse("0 0 0 15"),
            Content = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Padding = Thickness.Parse("5"),
                        // Background = new SolidColorBrush(state.IsDone ? Colors.Green : Colors.Red),
                        [!TemplatedControl.BackgroundProperty] = new Binding()
                        {
                            Source = state, Path = nameof(state.IsDone),
                            Converter = new FuncValueConverter<bool, IBrush>(b =>
                                new SolidColorBrush(state.IsDone ? Colors.Green : Colors.Red))
                        },
                        Content = new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitions
                            {
                                new ColumnDefinition(GridLength.Star),
                                new ColumnDefinition(GridLength.Auto)
                            },
                            Children =
                            {
                                new StackPanel
                                {
                                    [Grid.ColumnProperty] = 0,
                                    Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new Border
                                        {
                                            Background = new SolidColorBrush(Colors.Transparent),
                                            Padding = new Thickness(5, 5, 8, 8),
                                            Child = new Path
                                            {
                                                Data = Geometry.Parse(
                                                    "M16,17H5V7H16L19.55,12M17.63,5.84C17.27,5.33 16.67,5 16,5H5A2,2 0 0,0 3,7V17A2,2 0 0,0 5,19H16C16.67,19 17.27,18.66 17.63,18.15L22,12L17.63,5.84Z"),
                                                Fill = new SolidColorBrush(Colors.Black),
                                                HorizontalAlignment = HorizontalAlignment.Center,
                                                VerticalAlignment = VerticalAlignment.Center,
                                            }
                                        },
                                        new TextBlock
                                        {
                                            Foreground = new SolidColorBrush(Colors.White),
                                            FontSize = 18,
                                            VerticalAlignment = VerticalAlignment.Center,
                                            [!TextBlock.TextProperty] = new Binding
                                                { Source = state, Path = nameof(state.Title) }
                                        },
                                    }
                                },
                                new CheckBox
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    [Grid.ColumnProperty] = 1,
                                    [!CheckBox.IsCheckedProperty] = new Binding
                                        { Source = state, Path = nameof(state.IsDone), Mode = BindingMode.OneWay },
                                    Command = ReactiveCommand.Create(() =>
                                        dispatch(ToDoActionCreator.doneAction(state.UniqueId)))
                                },
                            }
                        }
                    },
                    new ContentControl
                    {
                        Background = new SolidColorBrush(Colors.LightGray),
                        Padding = Thickness.Parse("15 10"),
                        Content = new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitions
                            {
                                new ColumnDefinition(GridLength.Star),
                                new ColumnDefinition(GridLength.Auto)
                            },
                            Children =
                            {
                                new TextBlock
                                {
                                    [Grid.ColumnProperty] = 0,
                                    Foreground = new SolidColorBrush(Colors.Black),
                                    VerticalAlignment = VerticalAlignment.Center,
                                    [!TextBlock.TextProperty] = new Binding
                                        { Source = state, Path = nameof(state.Desc) }
                                },
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
                                        dispatch(ToDoActionCreator.onEditAction(state.UniqueId)))
                                }
                            }
                        }
                    }
                }
            }
        })
    {
    }
}