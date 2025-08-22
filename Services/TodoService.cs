using System.Text.Json;
using TodoApp_Maui.Models;

namespace TodoApp_Maui.Services;

public class TodoService : ITodoService
{
    private List<TodoItem> _todos = new();
    private int _nextId = 1;
    private readonly string _todosFileName = "todos.json";
    private string TodosFilePath => Path.Combine(FileSystem.AppDataDirectory, _todosFileName);

    public TodoService()
    {
        _ = LoadTodosAsync();
    }

    public async Task<List<TodoItem>> GetAllTodosAsync()
    {
        await LoadTodosAsync();
        return _todos.OrderByDescending(t => t.CreatedDate).ToList();
    }

    public async Task<TodoItem?> GetTodoByIdAsync(int id)
    {
        await LoadTodosAsync();
        return _todos.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TodoItem> AddTodoAsync(TodoItem todo)
    {
        await LoadTodosAsync();
        todo.Id = _nextId++;
        todo.CreatedDate = DateTime.Now;
        _todos.Add(todo);
        await SaveTodosAsync();
        return todo;
    }

    public async Task<TodoItem> UpdateTodoAsync(TodoItem todo)
    {
        await LoadTodosAsync();
        var existingTodo = _todos.FirstOrDefault(t => t.Id == todo.Id);
        if (existingTodo != null)
        {
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;
            if (todo.IsCompleted && !existingTodo.IsCompleted)
                existingTodo.CompletedDate = DateTime.Now;
            else if (!todo.IsCompleted)
                existingTodo.CompletedDate = null;
            
            await SaveTodosAsync();
            return existingTodo;
        }
        throw new ArgumentException("Todo not found");
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        await LoadTodosAsync();
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo != null)
        {
            _todos.Remove(todo);
            await SaveTodosAsync();
            return true;
        }
        return false;
    }

    public async Task<List<TodoItem>> GetTodosByStatusAsync(bool isCompleted)
    {
        await LoadTodosAsync();
        return _todos.Where(t => t.IsCompleted == isCompleted)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
    }

    public async Task SaveTodosAsync()
    {
        try
        {
            var todosData = new
            {
                NextId = _nextId,
                Todos = _todos
            };
            
            var json = JsonSerializer.Serialize(todosData, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            await File.WriteAllTextAsync(TodosFilePath, json);
        }
        catch (Exception ex)
        {
            // Log error - in a real app you would use proper logging
            System.Diagnostics.Debug.WriteLine($"Error saving todos: {ex.Message}");
        }
    }

    public async Task LoadTodosAsync()
    {
        try
        {
            if (File.Exists(TodosFilePath))
            {
                var json = await File.ReadAllTextAsync(TodosFilePath);
                if (!string.IsNullOrEmpty(json))
                {
                    var options = new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    };
                    
                    var todosData = JsonSerializer.Deserialize<dynamic>(json, options);
                    if (todosData != null)
                    {
                        var dataElement = (JsonElement)todosData;
                        
                        if (dataElement.TryGetProperty("NextId", out var nextIdElement))
                            _nextId = nextIdElement.GetInt32();
                        
                        if (dataElement.TryGetProperty("Todos", out var todosElement))
                        {
                            _todos = JsonSerializer.Deserialize<List<TodoItem>>(todosElement.GetRawText(), options) ?? new List<TodoItem>();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log error - in a real app you would use proper logging
            System.Diagnostics.Debug.WriteLine($"Error loading todos: {ex.Message}");
            _todos = new List<TodoItem>();
        }
    }
}