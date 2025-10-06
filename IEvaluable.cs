public interface IEvaluable
{
    void AddGrade(string studentId, string courseId, int grade);
    Dictionary<string, int> GetGrades(string studentId, string courseId);
    bool HasGrade(string studentId, string courseId);
}