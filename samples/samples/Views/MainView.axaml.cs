using Avalonia.Controls;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Content = Routes.routes.home;

            Navigator.onGenerateRoute = (RouteSettings settings) =>
            {
                var page = Routes.routes.buildPage(settings.name, settings.arguments);
                Content = page;
                return page;
            };
        }
    }
}