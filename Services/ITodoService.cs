using TodoApp_Maui.Models;

namespace TodoApp_Maui.Services;

public interface ITodoService
{
    Task<List<TodoItem>> GetAllTodosAsync();
    Task<TodoItem?> GetTodoByIdAsync(int id);
    Task<TodoItem> AddTodoAsync(TodoItem todo);
    Task<TodoItem> UpdateTodoAsync(TodoItem todo);
    Task<bool> DeleteTodoAsync(int id);
    Task<List<TodoItem>> GetTodosByStatusAsync(bool isCompleted);
    Task SaveTodosAsync();
    Task LoadTodosAsync();
}