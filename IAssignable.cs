public interface IAssignable
{
    void AssignStudent(string studentId, string courseId);
    void RemoveStudent(string studentId, string courseId);
    bool IsStudentAssigned(string studentId, string courseId);
}