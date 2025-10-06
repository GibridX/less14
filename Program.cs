namespace VirtualSchool
{
    class Program
    {
        static void Main()
        {
            VirtualSchool school = new VirtualSchool("data/school_data.txt");

            try
            {
                school.LoadDataFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
            }

            while (true)
            {
                Console.WriteLine("\nВиртуальная школа - Главное меню:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Добавить курс");
                Console.WriteLine("4. Записать студента на курс");
                Console.WriteLine("5. Добавить оценку студенту");
                Console.WriteLine("6. Показать студентов курса");
                Console.WriteLine("7. Показать всех пользователей");
                Console.WriteLine("8. Показать все курсы");
                Console.WriteLine("9. Сохранить данные");
                Console.WriteLine("10. Выход");

                Console.Write("Выберите действие: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddStudent(school);
                            break;
                        case "2":
                            AddTeacher(school);
                            break;
                        case "3":
                            AddCourse(school);
                            break;
                        case "4":
                            EnrollStudent(school);
                            break;
                        case "5":
                            AddGrade(school);
                            break;
                        case "6":
                            DisplayCourseStudents(school);
                            break;
                        case "7":
                            school.DisplayAllUsers();
                            break;
                        case "8":
                            school.DisplayAllCourses();
                            break;
                        case "9":
                            school.SaveDataToFile();
                            Console.WriteLine("Данные сохранены успешно");
                            break;
                        case "10":
                            school.SaveDataToFile();
                            Console.WriteLine("Данные сохранены. Выход из программы.");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void AddStudent(VirtualSchool school)
        {
            Console.Write("Введите ID студента: ");
            string? id = Console.ReadLine();
            Console.Write("Введите имя студента: ");
            string? name = Console.ReadLine();
            Console.Write("Введите email студента: ");
            string? email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Все поля должны быть заполнены");
                return;
            }

            school.AddUser(new Student 
            { 
                Id = id.Trim(), 
                Name = name.Trim(), 
                Email = email.Trim(), 
                EnrollmentDate = DateTime.Now 
            });
            Console.WriteLine("Студент добавлен успешно");
        }

        static void AddTeacher(VirtualSchool school)
        {
            Console.Write("Введите ID преподавателя: ");
            string? id = Console.ReadLine();
            Console.Write("Введите имя преподавателя: ");
            string? name = Console.ReadLine();
            Console.Write("Введите email преподавателя: ");
            string? email = Console.ReadLine();
            Console.Write("Введите специализацию: ");
            string? specialization = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name) || 
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(specialization))
            {
                Console.WriteLine("Все поля должны быть заполнены");
                return;
            }

            school.AddUser(new Teacher 
            { 
                Id = id.Trim(), 
                Name = name.Trim(), 
                Email = email.Trim(), 
                Specialization = specialization.Trim() 
            });
            Console.WriteLine("Преподаватель добавлен успешно");
        }

        static void AddCourse(VirtualSchool school)
        {
            Console.Write("Введите ID курса: ");
            string? id = Console.ReadLine();
            Console.Write("Введите название курса: ");
            string? title = Console.ReadLine();
            Console.Write("Введите описание курса: ");
            string? description = Console.ReadLine();
            Console.Write("Введите ID преподавателя: ");
            string? instructorId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(title) || 
                string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(instructorId))
            {
                Console.WriteLine("Все поля должны быть заполнены");
                return;
            }

            school.AddCourse(new OnlineCourse 
            { 
                CourseId = id.Trim(), 
                Title = title.Trim(), 
                Description = description.Trim(), 
                InstructorId = instructorId.Trim() 
            });
            Console.WriteLine("Курс добавлен успешно");
        }

        static void EnrollStudent(VirtualSchool school)
        {
            Console.Write("Введите ID студента: ");
            string? studentId = Console.ReadLine();
            Console.Write("Введите ID курса: ");
            string? courseId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(courseId))
            {
                Console.WriteLine("ID студента и ID курса не могут быть пустыми");
                return;
            }

            school.EnrollStudent(studentId, courseId);
            Console.WriteLine("Студент записан на курс успешно");
        }

        static void AddGrade(VirtualSchool school)
        {
            Console.Write("Введите ID студента: ");
            string? studentId = Console.ReadLine();
            Console.Write("Введите ID курса: ");
            string? courseId = Console.ReadLine();
            Console.Write("Введите оценку (1-5): ");
            string? gradeInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(gradeInput))
            {
                Console.WriteLine("Все поля должны быть заполнены");
                return;
            }

            if (int.TryParse(gradeInput, out int grade))
            {
                school.AddStudentGrade(studentId, courseId, grade);
                Console.WriteLine("Оценка добавлена успешно");
            }
            else
            {
                Console.WriteLine("Оценка должна быть числом");
            }
        }

        static void DisplayCourseStudents(VirtualSchool school)
        {
            Console.Write("Введите ID курса: ");
            string? courseId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(courseId))
            {
                Console.WriteLine("ID курса не может быть пустым");
                return;
            }

            school.DisplayCourseStudents(courseId);
        }
    }
}