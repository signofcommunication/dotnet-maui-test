using TodoApp_Maui.ViewModels;

namespace TodoApp_Maui.Views;

public partial class AddTodoPage : ContentPage
{
    private readonly AddTodoViewModel _viewModel;

    public AddTodoPage(AddTodoViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ResetForm();
    }
}