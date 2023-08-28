using Avalonia.Controls;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            WidgetWrapper content = new Counter.CounterPage().buildPage(null) as WidgetWrapper; //.create().build().create().build();
            this.Content = content.Content;
            //this.Content = new StackPanel
            //{
            //    Children =
            //    {
            //        new TextBlock
            //        {
            //            Text = "111"
            //        }
            //    }
            //};
        }
    }
}