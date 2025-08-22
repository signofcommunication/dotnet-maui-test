using TodoApp_Maui.Views;

namespace TodoApp_Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("addtodo", typeof(AddTodoPage));
        Routing.RegisterRoute("edittodo", typeof(EditTodoPage));
    }
}
