using System.ComponentModel.DataAnnotations;

namespace TodoApp_Maui.Models;

public class TodoItem
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public DateTime? CompletedDate { get; set; }
    
    public void MarkAsCompleted()
    {
        IsCompleted = true;
        CompletedDate = DateTime.Now;
    }
    
    public void MarkAsIncomplete()
    {
        IsCompleted = false;
        CompletedDate = null;
    }
}