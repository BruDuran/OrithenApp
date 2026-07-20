namespace OrithenApp.Models;

public class Station
{
    public int Id { get; set; }
    public int BlockId { get; set; }
    public int Number { get; set; }
    public string Material { get; set; } = string.Empty;
    public int RotationOrder { get; set; }
}
