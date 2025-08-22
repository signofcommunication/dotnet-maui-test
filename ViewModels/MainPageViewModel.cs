using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp_Maui.Models;
using TodoApp_Maui.Services;

namespace TodoApp_Maui.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;
    private ObservableCollection<TodoItem> _todos = new();
    private string _filterStatus = "All";

    public MainPageViewModel(ITodoService todoService)
    {
        _todoService = todoService;
        Title = "Todo List";
        
        LoadTodosCommand = new RelayCommand(async () => await LoadTodosAsync());
        AddTodoCommand = new RelayCommand(async () => await AddTodoAsync());
        EditTodoCommand = new RelayCommand<TodoItem>(async (todo) => await EditTodoAsync(todo));
        DeleteTodoCommand = new RelayCommand<TodoItem>(async (todo) => await DeleteTodoAsync(todo));
        ToggleCompletedCommand = new RelayCommand<TodoItem>(async (todo) => await ToggleCompletedAsync(todo));
        FilterTodosCommand = new RelayCommand<string>(async (filter) => await FilterTodosAsync(filter));
        
        _ = LoadTodosAsync();
    }

    public ObservableCollection<TodoItem> Todos
    {
        get => _todos;
        set => SetProperty(ref _todos, value);
    }

    public string FilterStatus
    {
        get => _filterStatus;
        set => SetProperty(ref _filterStatus, value);
    }

    public ICommand LoadTodosCommand { get; }
    public ICommand AddTodoCommand { get; }
    public ICommand EditTodoCommand { get; }
    public ICommand DeleteTodoCommand { get; }
    public ICommand ToggleCompletedCommand { get; }
    public ICommand FilterTodosCommand { get; }

    private async Task LoadTodosAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var todos = await _todoService.GetAllTodosAsync();
            
            Todos.Clear();
            foreach (var todo in todos)
            {
                Todos.Add(todo);
            }
            
            await ApplyCurrentFilter();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to load todos: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddTodoAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("addtodo");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Navigation error: {ex.Message}", "OK");
        }
    }

    private async Task EditTodoAsync(TodoItem? todo)
    {
        if (todo == null) return;

        try
        {
            await Shell.Current.GoToAsync($"edittodo?todoid={todo.Id}");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Navigation error: {ex.Message}", "OK");
        }
    }

    private async Task DeleteTodoAsync(TodoItem? todo)
    {
        if (todo == null) return;

        try
        {
            var result = await Application.Current!.MainPage!.DisplayAlert(
                "Delete Todo", 
                $"Are you sure you want to delete '{todo.Title}'?", 
                "Yes", 
                "No");

            if (result)
            {
                await _todoService.DeleteTodoAsync(todo.Id);
                Todos.Remove(todo);
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to delete todo: {ex.Message}", "OK");
        }
    }

    private async Task ToggleCompletedAsync(TodoItem? todo)
    {
        if (todo == null) return;

        try
        {
            todo.IsCompleted = !todo.IsCompleted;
            if (todo.IsCompleted)
                todo.MarkAsCompleted();
            else
                todo.MarkAsIncomplete();

            await _todoService.UpdateTodoAsync(todo);
            
            // Refresh the current filter
            await ApplyCurrentFilter();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to update todo: {ex.Message}", "OK");
        }
    }

    private async Task FilterTodosAsync(string? filter)
    {
        if (string.IsNullOrEmpty(filter)) return;

        FilterStatus = filter;
        await ApplyCurrentFilter();
    }

    private async Task ApplyCurrentFilter()
    {
        try
        {
            List<TodoItem> filteredTodos = FilterStatus switch
            {
                "Completed" => await _todoService.GetTodosByStatusAsync(true),
                "Pending" => await _todoService.GetTodosByStatusAsync(false),
                _ => await _todoService.GetAllTodosAsync()
            };

            Todos.Clear();
            foreach (var todo in filteredTodos)
            {
                Todos.Add(todo);
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Unable to filter todos: {ex.Message}", "OK");
        }
    }

    public async Task RefreshAsync()
    {
        await LoadTodosAsync();
    }
}