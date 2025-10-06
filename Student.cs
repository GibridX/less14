namespace VirtualSchool
{
    public class Student : User
    {
        public DateTime EnrollmentDate { get; set; }

        public Student() : base() { }

        public Student(string id, string name, string email, DateTime enrollmentDate) 
            : base(id, name, email)
        {
            EnrollmentDate = enrollmentDate;
        }

        public override string ToFileString()
        {
            return $"{base.ToFileString()}|{EnrollmentDate:yyyy-MM-dd}";
        }
    }
}