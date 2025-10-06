namespace VirtualSchool
{
    public class VirtualSchool
    {
        private Dictionary<string, Course> courses;
        private Dictionary<string, User> users;
        private Dictionary<string, OnlineCourse> onlineCourses;
        private FileDataService _fileDataService;

        public VirtualSchool(string dataFilePath = "school_data.txt")
        {
            courses = new Dictionary<string, Course>();
            users = new Dictionary<string, User>();
            onlineCourses = new Dictionary<string, OnlineCourse>();
            _fileDataService = new FileDataService(dataFilePath);
        }

        public void AddUser(User user)
        {
            if (user?.Id != null && !string.IsNullOrWhiteSpace(user.Id))
            {
                users[user.Id] = user;
            }
            else
            {
                throw new ArgumentException("ID пользователя не может быть пустым");
            }
        }

        public void AddCourse(Course course)
        {
            if (course?.CourseId != null && !string.IsNullOrWhiteSpace(course.CourseId))
            {
                courses[course.CourseId] = course;
                
                if (course is OnlineCourse onlineCourse)
                {
                    onlineCourses[course.CourseId] = onlineCourse;
                }
                else
                {
                    var newOnlineCourse = new OnlineCourse
                    {
                        CourseId = course.CourseId,
                        Title = course.Title,
                        Description = course.Description,
                        InstructorId = course.InstructorId
                    };
                    onlineCourses[course.CourseId] = newOnlineCourse;
                }
            }
            else
            {
                throw new ArgumentException("ID курса не может быть пустым");
            }
        }

        public void EnrollStudent(string studentId, string courseId)
        {
            ValidateStudentAndCourse(studentId, courseId);

            if (!onlineCourses.ContainsKey(courseId))
            {
                throw new ArgumentException($"Курс с ID {courseId} не инициализирован правильно");
            }

            onlineCourses[courseId].AssignStudent(studentId, courseId);
        }

        public void AddStudentGrade(string studentId, string courseId, int grade)
        {
            ValidateStudentAndCourse(studentId, courseId);

            if (grade < 1 || grade > 5)
            {
                throw new ArgumentException("Оценка должна быть от 1 до 5");
            }

            if (!onlineCourses.ContainsKey(courseId))
            {
                throw new ArgumentException($"Курс с ID {courseId} не инициализирован правильно");
            }

            if (!onlineCourses[courseId].IsStudentAssigned(studentId, courseId))
            {
                throw new ArgumentException($"Студент {studentId} не записан на курс {courseId}");
            }

            onlineCourses[courseId].AddGrade(studentId, courseId, grade);
        }

        public void DisplayCourseStudents(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                Console.WriteLine("ID курса не может быть пустым");
                return;
            }

            if (!onlineCourses.ContainsKey(courseId))
            {
                Console.WriteLine($"Курс с ID {courseId} не существует");
                return;
            }

            var students = onlineCourses[courseId].GetEnrolledStudents(courseId);
            
            if (students.Count > 0)
            {
                Console.WriteLine($"Студенты курса {courseId}:");
                foreach (var studentId in students)
                {
                    if (users.ContainsKey(studentId))
                    {
                        Console.WriteLine($"- {users[studentId].Name} (ID: {studentId})");
                        
                        var grades = onlineCourses[courseId].GetGrades(studentId, courseId);
                        if (grades.ContainsKey(courseId))
                        {
                            Console.WriteLine($"  Оценка: {grades[courseId]}");
                        }
                        else
                        {
                            Console.WriteLine($"  Оценка: не выставлена");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"На курсе {courseId} нет студентов");
            }
        }

        public void DisplayAllUsers()
        {
            Console.WriteLine("\nВсе пользователи:");
            foreach (var user in users.Values)
            {
                if (user is Student student)
                {
                    Console.WriteLine($"Студент: {student.Name} (ID: {student.Id}, Email: {student.Email})");
                }
                else if (user is Teacher teacher)
                {
                    Console.WriteLine($"Преподаватель: {teacher.Name} (ID: {teacher.Id}, Специализация: {teacher.Specialization})");
                }
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\nВсе курсы:");
            foreach (var course in courses.Values)
            {
                var enrolledCount = onlineCourses.ContainsKey(course.CourseId) ? 
                    onlineCourses[course.CourseId].GetEnrolledStudents(course.CourseId).Count : 0;
                
                Console.WriteLine($"Курс: {course.Title} (ID: {course.CourseId}, Студентов: {enrolledCount})");
            }
        }

        public void SaveDataToFile()
        {
            try
            {
                var schoolData = CollectSchoolData();
                _fileDataService.SaveData(schoolData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении данных: {ex.Message}", ex);
            }
        }

        public void LoadDataFromFile()
        {
            try
            {
                var schoolData = _fileDataService.LoadData();
                
                // Очищаем текущие данные
                courses.Clear();
                users.Clear();
                onlineCourses.Clear();

                // Загружаем пользователей
                foreach (var user in schoolData.Users)
                {
                    if (user?.Id != null)
                    {
                        AddUser(user);
                    }
                }

                // Загружаем курсы
                foreach (var course in schoolData.Courses)
                {
                    if (course?.CourseId != null)
                    {
                        AddCourse(course);
                    }
                }

                // Восстанавливаем записи на курсы
                foreach (var enrollment in schoolData.Enrollments)
                {
                    var courseId = enrollment.Key;
                    if (onlineCourses.ContainsKey(courseId))
                    {
                        foreach (var studentId in enrollment.Value)
                        {
                            onlineCourses[courseId].AssignStudent(studentId, courseId);
                        }
                    }
                }

                // Восстанавливаем оценки
                foreach (var studentGrade in schoolData.Grades)
                {
                    var studentId = studentGrade.Key;
                    foreach (var courseGrade in studentGrade.Value)
                    {
                        var courseId = courseGrade.Key;
                        var grade = courseGrade.Value;
                        
                        if (onlineCourses.ContainsKey(courseId))
                        {
                            onlineCourses[courseId].AddGrade(studentId, courseId, grade);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке данных: {ex.Message}", ex);
            }
        }

        private SchoolData CollectSchoolData()
        {
            return new SchoolData
            {
                Users = new List<User>(users.Values),
                Courses = new List<Course>(courses.Values),
                Enrollments = onlineCourses.Values
                    .SelectMany(oc => oc.CourseStudents)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Grades = onlineCourses.Values
                    .SelectMany(oc => oc.StudentGrades)
                    .GroupBy(kvp => kvp.Key)
                    .ToDictionary(g => g.Key, g => g.SelectMany(x => x.Value).ToDictionary(x => x.Key, x => x.Value))
            };
        }

        private void ValidateStudentAndCourse(string studentId, string courseId)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId))
            {
                throw new ArgumentException("ID студента и ID курса не могут быть пустыми");
            }

            if (!courses.ContainsKey(courseId))
            {
                throw new ArgumentException($"Курс с ID {courseId} не существует");
            }

            if (!users.ContainsKey(studentId) || users[studentId] is not Student)
            {
                throw new ArgumentException($"Студент с ID {studentId} не существует");
            }

            var course = courses[courseId];
            if (!string.IsNullOrEmpty(course.InstructorId) && !users.ContainsKey(course.InstructorId))
            {
                throw new ArgumentException($"Преподаватель с ID {course.InstructorId} для курса {courseId} не существует");
            }
        }
    }
}