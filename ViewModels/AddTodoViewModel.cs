using System.Windows.Input;
using TodoApp_Maui.Models;
using TodoApp_Maui.Services;

namespace TodoApp_Maui.ViewModels;

public class AddTodoViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;
    private string _todoTitle = string.Empty;
    private string _todoDescription = string.Empty;

    public AddTodoViewModel(ITodoService todoService)
    {
        _todoService = todoService;
        Title = "Add Todo";
        
        SaveCommand = new RelayCommand(async () => await SaveTodoAsync(), CanSave);
        CancelCommand = new RelayCommand(async () => await CancelAsync());
    }

    public string TodoTitle
    {
        get => _todoTitle;
        set
        {
            SetProperty(ref _todoTitle, value);
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    public string TodoDescription
    {
        get => _todoDescription;
        set => SetProperty(ref _todoDescription, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(TodoTitle) && !IsBusy;
    }

    private async Task SaveTodoAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            
            var newTodo = new TodoItem
            {
                Title = TodoTitle.Trim(),
                Description = TodoDescription.Trim(),
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };

            await _todoService.AddTodoAsync(newTodo);
            
            // Navigate back to main page
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to save todo: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CancelAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Navigation error: {ex.Message}", "OK");
        }
    }

    public void ResetForm()
    {
        TodoTitle = string.Empty;
        TodoDescription = string.Empty;
    }
}