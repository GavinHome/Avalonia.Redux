﻿namespace samples.Pages.Todos.Report;

internal class ReportComponent() : Component<ReportState>(
    view: (state, dispatch, _) => new View() { DataContext = state });


/// [C# for View]
/// You can write UI in C#, as the following code, and equivalent to build UI using XAML. 
/// I found that more verbose when writing UI in C#, and binding properties will be more troublesome. 
/// However, the most convenient way to build UI using XAML.
/// Property, it is not very convenient to bind Command.
/// You need to register StyledProperty in the background C# file of XAML in advance.
#region
////internal class ReportComponent() : Component<ReportState>(
////view: (state, dispatch, _) => new ContentControl
////{
////    Background = new SolidColorBrush(Colors.DodgerBlue),
////    Foreground = new SolidColorBrush(Colors.White),
////    Content = new StackPanel
////    {
////        Orientation = Orientation.Horizontal,
////        VerticalAlignment = VerticalAlignment.Center,
////        Children =
////    {
////        new Border
////        {
////            Background = new SolidColorBrush(Colors.Transparent),
////            Padding = new Thickness(5, 5, 8, 8),
////            Child = new Path
////            {
////                Data = Geometry.Parse(
////                    "M13 13H11V7H13M11 15H13V17H11M15.73 3H8.27L3 8.27V15.73L8.27 21H15.73L21 15.73V8.27L15.73 3Z"),
////                Fill = new SolidColorBrush(Colors.Black),
////                HorizontalAlignment = HorizontalAlignment.Center,
////                VerticalAlignment = VerticalAlignment.Center,
////            }
////        },
////        new ContentControl
////        {
////            Content = new StackPanel
////            {
////                Orientation = Orientation.Horizontal,
////                HorizontalAlignment = HorizontalAlignment.Center,
////                VerticalAlignment = VerticalAlignment.Center,
////                Children =
////                {
////                    new TextBlock
////                    {
////                        Text = "Total ",
////                    },
////                    new TextBlock
////                    {
////                        [!TextBlock.TextProperty] = new Binding
////                            { Source = state, Path = nameof(state.Total) }
////                    },
////                    new TextBlock
////                    {
////                        Text = " tasks, ",
////                    },
////                    new TextBlock
////                    {
////                        [!TextBlock.TextProperty] = new Binding
////                            { Source = state, Path = nameof(state.Done) }
////                    },
////                    new TextBlock
////                    {
////                        Text = " done. ",
////                    },
////                }
////            }
////        }
////    }
////    }
////});
#endregion