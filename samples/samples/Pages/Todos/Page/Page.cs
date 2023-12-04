﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using samples.Pages.Todos.Report;
using samples.Views;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

namespace samples.Pages.Todos.Page;
using Action = Redux.Action;

public partial class ToDoListPage : Page<PageState, Dictionary<string, dynamic>>
{
    public ToDoListPage() : base(
        initState: initState,
        effect: buildEffect(),
        reducer: buildReducer(),
        middlewares:
        [
            Redux.Middlewares.logMiddleware<PageState>(monitor: (state) => state.ToString(), tag: "ToDoListPage")
        ],
        dependencies: new Dependencies<PageState>(
            adapter: new NoneConn<PageState>() + new PageAdapter(),
            slots: new Dictionary<String, Dependent<PageState>>
            {
                { "report", new ReportConnector() + new ReportComponent() },
            }
        ),
        view: (state, dispatch, ctx) =>
        {
            ////var todos = ctx.buildComponents();
            var report = ctx.buildComponent("report");
            var itemsView = buildItemsView(state.ToDos!, ctx);

            return new DockPanel
            {
                [Grid.IsSharedSizeScopeProperty] = true,
                Children =
                {
                    new StackPanel
                    {
                        [DockPanel.DockProperty] = Dock.Bottom,
                        [Visual.ZIndexProperty] = 99,
                        Children =
                        {
                            new Border
                            {
                                Padding = Thickness.Parse("8"),
                                Child = new Grid
                                {
                                    Children =
                                    {
                                        new Border
                                        {
                                            Child = report,
                                        },
                                        new Border
                                        {
                                            Margin = Thickness.Parse("0 -20 10 8"),
                                            HorizontalAlignment = HorizontalAlignment.Right,
                                            VerticalAlignment = VerticalAlignment.Center,
                                            Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                            Padding = new Thickness(0),
                                            CornerRadius = new CornerRadius(15),
                                            Child = new Button()
                                            {
                                                Background = SolidColorBrush.Parse("#bbe9d3ff"),
                                                CornerRadius = new CornerRadius(15),
                                                Padding = new Thickness(0),
                                                Height = 50, Width = 50,
                                                BorderThickness = new Thickness(0),
                                                Content = new Border
                                                {
                                                    Padding = new Thickness(8, 5, 12, 8),
                                                    Child = new Path
                                                    {
                                                        Data = Geometry.Parse(
                                                            "M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z"),
                                                        Fill = new SolidColorBrush(Colors.Black),
                                                        HorizontalAlignment = HorizontalAlignment.Center,
                                                        VerticalAlignment = VerticalAlignment.Center,
                                                    },
                                                },
                                                Command = ReactiveCommand.Create(() =>
                                                    dispatch(new Action("onAdd")))
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new StackPanel
                    {
                        Children =
                        {
                            new Grid
                            {
                                Margin = Thickness.Parse("8"),
                                Children =
                                {
                                    new ScrollViewer
                                    {
                                        [!Layoutable.HeightProperty] = new Binding()
                                        {
                                            Path = "Height",
                                            RelativeSource = new RelativeSource()
                                            {
                                                Mode = RelativeSourceMode.FindAncestor,
                                                AncestorType = typeof(MainWindow)
                                            }
                                        },
                                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                        Content = itemsView
                                    }
                                }
                            },
                        }
                    }
                }
            };
        })
    {
    }

    private static PageState initState(Dictionary<string, dynamic>? param) => new() { ToDos = [] };

    private static ItemsControl buildItemsView<T>(ObservableCollection<T> obs, ComponentContext<PageState> ctx)
    {
        var source = new Subject<List<Control>>();
        var items = new ItemsControl()
        {
            [!ItemsControl.ItemsSourceProperty] = source.ToBinding()
        };

        obs.ToObservableChangeSet().Subscribe(x =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var todos = ctx.buildComponents();
                source.OnNext(todos);
            });
        });

        return items;
    }
}
