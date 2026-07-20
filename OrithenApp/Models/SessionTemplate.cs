namespace OrithenApp.Models;

public class SessionTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Warmup { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
