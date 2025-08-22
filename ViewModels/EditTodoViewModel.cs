using System.Windows.Input;
using TodoApp_Maui.Models;
using TodoApp_Maui.Services;

namespace TodoApp_Maui.ViewModels;

[QueryProperty(nameof(TodoId), "todoid")]
public class EditTodoViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;
    private TodoItem? _originalTodo;
    private int _todoId;
    private string _todoTitle = string.Empty;
    private string _todoDescription = string.Empty;
    private bool _isCompleted;

    public EditTodoViewModel(ITodoService todoService)
    {
        _todoService = todoService;
        Title = "Edit Todo";
        
        SaveCommand = new RelayCommand(async () => await SaveTodoAsync(), CanSave);
        CancelCommand = new RelayCommand(async () => await CancelAsync());
        DeleteCommand = new RelayCommand(async () => await DeleteTodoAsync());
    }

    public int TodoId
    {
        get => _todoId;
        set
        {
            SetProperty(ref _todoId, value);
            _ = LoadTodoAsync();
        }
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

    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand DeleteCommand { get; }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(TodoTitle) && !IsBusy;
    }

    private async Task LoadTodoAsync()
    {
        if (TodoId <= 0) return;

        try
        {
            IsBusy = true;
            _originalTodo = await _todoService.GetTodoByIdAsync(TodoId);
            
            if (_originalTodo != null)
            {
                TodoTitle = _originalTodo.Title;
                TodoDescription = _originalTodo.Description;
                IsCompleted = _originalTodo.IsCompleted;
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Todo not found", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to load todo: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveTodoAsync()
    {
        if (IsBusy || _originalTodo == null) return;

        try
        {
            IsBusy = true;
            
            _originalTodo.Title = TodoTitle.Trim();
            _originalTodo.Description = TodoDescription.Trim();
            
            if (IsCompleted != _originalTodo.IsCompleted)
            {
                if (IsCompleted)
                    _originalTodo.MarkAsCompleted();
                else
                    _originalTodo.MarkAsIncomplete();
            }

            await _todoService.UpdateTodoAsync(_originalTodo);
            
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

    private async Task DeleteTodoAsync()
    {
        if (_originalTodo == null) return;

        try
        {
            var result = await Application.Current!.MainPage!.DisplayAlert(
                "Delete Todo", 
                $"Are you sure you want to delete '{_originalTodo.Title}'?", 
                "Yes", 
                "No");

            if (result)
            {
                await _todoService.DeleteTodoAsync(_originalTodo.Id);
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to delete todo: {ex.Message}", "OK");
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
}