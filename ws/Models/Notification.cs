namespace ws.Models;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
