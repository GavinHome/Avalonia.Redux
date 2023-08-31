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

            this.Content = new CounterPage().buildPage(null);
            this.Content = new ToDoListPage().buildPage(null);
        }

        AbstractRoutes routes = new PageRoutes(
            initialRoute: "count",
            pages: new Dictionary<String, Page<Object, dynamic>>
            {
                //// Register TodoList main page
                //// { "todo_list", new ToDoListPage()},

                //// Register Todo edit page
                //// { "todo_edit", new TodoEditPage()},

                //// Register Count page
                //// { "count", new CounterPage() },
            });
    }
}