using Avalonia.Controls;
using samples.Pages.Counter;
using samples.Pages.Todos.Page;

namespace samples.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            //WidgetWrapper? content = new CounterPage().buildPage(null) as dynamic;           
            WidgetWrapper? content = new ToDoListPage().buildPage(null) as dynamic;
            this.Content = content!.Content;
        }

        ////AbstractRoutes routes = new PageRoutes(
        ////    initialRoute: "count",
        ////    pages: new Dictionary<String, Page<Object, dynamic>>
        ////    {
        ////        /// Register TodoList main page
        ////        //{ "todo_list", new ToDoListPage()},

        ////        /// Register Todo edit page
        ////        //{ "todo_edit", new TodoEditPage()},

        ////        /// Register Count page
        ////        //{ "count", new CounterPage() },
        ////    });
    }
}