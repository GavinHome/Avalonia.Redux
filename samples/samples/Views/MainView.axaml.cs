using Avalonia.Controls;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            Navigator.onGenerateRoute = settings => Routes.routes.buildPage(settings.name, settings.arguments);
            Navigator.onRouteChanged = route => Content = route!.Content;
            Routes.routes.buildHome();
        }
    }
}