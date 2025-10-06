namespace VirtualSchool
{
    public class OnlineCourse : Course, IAssignable, IEvaluable
    {
        public Dictionary<string, List<string>> CourseStudents { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, Dictionary<string, int>> StudentGrades { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        public OnlineCourse() : base() { }

        public OnlineCourse(string courseId, string title, string description, string instructorId) 
            : base(courseId, title, description, instructorId) { }

        public void AssignStudent(string studentId, string courseId)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId)) 
                return;

            if (!CourseStudents.ContainsKey(courseId))
            {
                CourseStudents[courseId] = new List<string>();
            }

            if (!CourseStudents[courseId].Contains(studentId))
            {
                CourseStudents[courseId].Add(studentId);
            }
        }

        public void RemoveStudent(string studentId, string courseId)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId)) 
                return;

            if (CourseStudents.ContainsKey(courseId))
            {
                CourseStudents[courseId].Remove(studentId);
                
                if (CourseStudents[courseId].Count == 0)
                {
                    CourseStudents.Remove(courseId);
                }
            }
        }

        public bool IsStudentAssigned(string studentId, string courseId)
        {
            return CourseStudents.ContainsKey(courseId) && 
                   CourseStudents[courseId].Contains(studentId);
        }

        public void AddGrade(string studentId, string courseId, int grade)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId)) 
                return;

            if (!StudentGrades.ContainsKey(studentId))
            {
                StudentGrades[studentId] = new Dictionary<string, int>();
            }

            StudentGrades[studentId][courseId] = grade;
        }

        public Dictionary<string, int> GetGrades(string studentId, string courseId)
        {
            var result = new Dictionary<string, int>();
            
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(courseId) && 
                StudentGrades.ContainsKey(studentId) && StudentGrades[studentId].ContainsKey(courseId))
            {
                result[courseId] = StudentGrades[studentId][courseId];
            }

            return result;
        }

        public bool HasGrade(string studentId, string courseId)
        {
            return StudentGrades.ContainsKey(studentId) && 
                   StudentGrades[studentId].ContainsKey(courseId);
        }

        public List<string> GetEnrolledStudents(string courseId)
        {
            return CourseStudents.ContainsKey(courseId) ? 
                   new List<string>(CourseStudents[courseId]) : 
                   new List<string>();
        }

        // Методы для работы с файлами
        public string EnrollmentsToFileString()
        {
            var lines = new List<string>();
            foreach (var enrollment in CourseStudents)
            {
                var studentIds = string.Join(",", enrollment.Value);
                lines.Add($"{enrollment.Key}:{studentIds}");
            }
            return string.Join(";", lines);
        }

        public void EnrollmentsFromFileString(string data)
        {
            CourseStudents.Clear();
            if (string.IsNullOrEmpty(data)) return;

            var enrollments = data.Split(';');
            foreach (var enrollment in enrollments)
            {
                var parts = enrollment.Split(':');
                if (parts.Length == 2)
                {
                    var courseId = parts[0];
                    var studentIds = parts[1].Split(',');
                    CourseStudents[courseId] = new List<string>(studentIds);
                }
            }
        }

        public string GradesToFileString()
        {
            var lines = new List<string>();
            foreach (var studentGrade in StudentGrades)
            {
                var gradeLines = new List<string>();
                foreach (var courseGrade in studentGrade.Value)
                {
                    gradeLines.Add($"{courseGrade.Key}={courseGrade.Value}");
                }
                lines.Add($"{studentGrade.Key}:{string.Join(",", gradeLines)}");
            }
            return string.Join(";", lines);
        }

        public void GradesFromFileString(string data)
        {
            StudentGrades.Clear();
            if (string.IsNullOrEmpty(data)) return;

            var studentGrades = data.Split(';');
            foreach (var studentGrade in studentGrades)
            {
                var parts = studentGrade.Split(':');
                if (parts.Length == 2)
                {
                    var studentId = parts[0];
                    var courseGrades = parts[1].Split(',');
                    
                    StudentGrades[studentId] = new Dictionary<string, int>();
                    foreach (var courseGrade in courseGrades)
                    {
                        var gradeParts = courseGrade.Split('=');
                        if (gradeParts.Length == 2 && int.TryParse(gradeParts[1], out int grade))
                        {
                            StudentGrades[studentId][gradeParts[0]] = grade;
                        }
                    }
                }
            }
        }
    }
}