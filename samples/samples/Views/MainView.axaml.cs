using Avalonia.Controls;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Content = Routes.routes.home;

            Navigator.onGenerateRoute = settings =>
            {
                var page = Routes.routes.buildPage(settings.name, settings.arguments);
                return page;
            };

            Navigator.onChange += () =>
            {
                Content = Navigator.of().current;
            };
        }
    }
}