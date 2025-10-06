namespace VirtualSchool
{
    public abstract class User
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        protected User() { }

        protected User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public virtual string ToFileString()
        {
            return $"{Id}|{Name}|{Email}";
        }

        public static User FromFileString(string data)
        {
            var parts = data.Split('|');
            if (parts.Length < 3) return null!;

            if (parts.Length == 4 && DateTime.TryParse(parts[3], out DateTime enrollmentDate))
            {
                return new Student(parts[0], parts[1], parts[2], enrollmentDate);
            }
            else if (parts.Length == 4)
            {
                return new Teacher(parts[0], parts[1], parts[2], parts[3]);
            }
            
            return null!;
        }
    }
}