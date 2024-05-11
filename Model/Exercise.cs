namespace API_Sport_Spirit.Model;

public partial class Exercise
{
    public int IdExercise { get; set; }

    public string ExerciseName { get; set; } = null!;

    public string? ExerciseDescriptions { get; set; }

    public string MuscleGroup { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? CollectionServerId { get; set; }

    public int ExerciseCriteriaId { get; set; }
}
