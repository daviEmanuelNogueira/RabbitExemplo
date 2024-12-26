using System.ComponentModel.DataAnnotations;

namespace ws.Models;

public class Notification
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
