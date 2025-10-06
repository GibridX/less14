namespace VirtualSchool
{
    public class Teacher : User
    {
        public string Specialization { get; set; } = string.Empty;

        public Teacher() : base() { }

        public Teacher(string id, string name, string email, string specialization) 
            : base(id, name, email)
        {
            Specialization = specialization;
        }

        public override string ToFileString()
        {
            return $"{base.ToFileString()}|{Specialization}";
        }
    }
}