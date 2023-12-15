namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            Navigator.build(
                routes: Routes.routes,
                routeChanged: route => Content = route!.Content
            );
            
            // Navigator.build(
            //     routes: Routes.routes,
            //     routeChanged: route => Content = route!.Content,
            //     generateRoute: settings => Routes.routes.buildPage(settings.name, settings.arguments)
            // );
        }
    }
}