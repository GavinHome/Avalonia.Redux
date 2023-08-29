using Avalonia.Controls;
using samples.Pages.Counter;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            WidgetWrapper? content = new CounterPage().buildPage(null) as dynamic;
            this.Content = content!.Content;
        }
    }
}