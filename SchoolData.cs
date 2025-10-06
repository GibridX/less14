namespace VirtualSchool
{
    public class SchoolData
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public Dictionary<string, List<string>> Enrollments { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, Dictionary<string, int>> Grades { get; set; } = new Dictionary<string, Dictionary<string, int>>();
    }
}