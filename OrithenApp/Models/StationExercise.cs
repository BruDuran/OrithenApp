namespace OrithenApp.Models;

public class StationExercise
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public int ExerciseId { get; set; }
    public string Series { get; set; } = string.Empty;
    public string Repetitions { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int Order { get; set; }
}
