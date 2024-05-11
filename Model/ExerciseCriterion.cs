namespace API_Sport_Spirit.Model;

public partial class ExerciseCriterion
{
    public int IdExerciseCriteria { get; set; }

    public TimeOnly? ExecutionTime { get; set; }

    public int? Approaches { get; set; }

    public int? Repetition { get; set; }

    public bool IsDeleted { get; set; }
}
