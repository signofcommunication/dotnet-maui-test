using TodoApp_Maui.ViewModels;

namespace TodoApp_Maui.Views;

public partial class EditTodoPage : ContentPage
{
    public EditTodoPage(EditTodoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}