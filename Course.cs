namespace VirtualSchool
{
    public abstract class Course
    {
        public string CourseId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string InstructorId { get; set; } = string.Empty;

        protected Course() { }

        protected Course(string courseId, string title, string description, string instructorId)
        {
            CourseId = courseId;
            Title = title;
            Description = description;
            InstructorId = instructorId;
        }

        public virtual string ToFileString()
        {
            return $"{CourseId}|{Title}|{Description}|{InstructorId}";
        }

        public static Course FromFileString(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 4)
            {
                return new OnlineCourse(parts[0], parts[1], parts[2], parts[3]);
            }
            return null!;
        }
    }
}