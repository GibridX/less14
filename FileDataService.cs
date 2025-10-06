using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirtualSchool
{
    public class FileDataService
    {
        private string _dataFilePath;

        public FileDataService(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public SchoolData LoadData()
        {
            var schoolData = new SchoolData();

            if (!File.Exists(_dataFilePath))
            {
                Console.WriteLine("Файл данных не найден. Будет создан новый при сохранении.");
                return schoolData;
            }

            try
            {
                string currentSection = "";
                
                foreach (string line in File.ReadAllLines(_dataFilePath))
                {
                    if (line.StartsWith("["))
                    {
                        currentSection = line.Trim();
                        continue;
                    }

                    if (string.IsNullOrEmpty(line)) continue;

                    ProcessLine(schoolData, currentSection, line);
                }

                Console.WriteLine("Данные успешно загружены из файла.");
                return schoolData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки данных из файла: {ex.Message}", ex);
            }
        }

        public void SaveData(SchoolData schoolData)
        {
            try
            {
                // Создаем директорию если не существует
                var directory = Path.GetDirectoryName(_dataFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(_dataFilePath))
                {
                    WriteUsersSection(writer, schoolData.Users);
                    WriteCoursesSection(writer, schoolData.Courses);
                    WriteEnrollmentsSection(writer, schoolData.Enrollments);
                    WriteGradesSection(writer, schoolData.Grades);
                }

                Console.WriteLine($"Данные успешно сохранены в файл: {_dataFilePath}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения данных в файл: {ex.Message}", ex);
            }
        }

        private void ProcessLine(SchoolData schoolData, string section, string line)
        {
            switch (section)
            {
                case "[USERS]":
                    var user = User.FromFileString(line);
                    if (user != null)
                    {
                        schoolData.Users.Add(user);
                    }
                    break;

                case "[COURSES]":
                    var course = Course.FromFileString(line);
                    if (course != null)
                    {
                        schoolData.Courses.Add(course);
                    }
                    break;

                case "[ENROLLMENTS]":
                    ProcessEnrollmentLine(schoolData, line);
                    break;

                case "[GRADES]":
                    ProcessGradeLine(schoolData, line);
                    break;
            }
        }

        private void ProcessEnrollmentLine(SchoolData schoolData, string line)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var courseId = parts[0];
                var studentIds = parts[1].Split(',').Where(id => !string.IsNullOrEmpty(id)).ToList();
                schoolData.Enrollments[courseId] = studentIds;
            }
        }

        private void ProcessGradeLine(SchoolData schoolData, string line)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var studentId = parts[0];
                var courseGrades = parts[1].Split(',');

                schoolData.Grades[studentId] = new Dictionary<string, int>();
                
                foreach (var courseGrade in courseGrades)
                {
                    var gradeParts = courseGrade.Split('=');
                    if (gradeParts.Length == 2 && int.TryParse(gradeParts[1], out int grade))
                    {
                        schoolData.Grades[studentId][gradeParts[0]] = grade;
                    }
                }
            }
        }

        private void WriteUsersSection(StreamWriter writer, List<User> users)
        {
            writer.WriteLine("[USERS]");
            foreach (var user in users)
            {
                writer.WriteLine(user.ToFileString());
            }
            writer.WriteLine();
        }

        private void WriteCoursesSection(StreamWriter writer, List<Course> courses)
        {
            writer.WriteLine("[COURSES]");
            foreach (var course in courses)
            {
                writer.WriteLine(course.ToFileString());
            }
            writer.WriteLine();
        }

        private void WriteEnrollmentsSection(StreamWriter writer, Dictionary<string, List<string>> enrollments)
        {
            writer.WriteLine("[ENROLLMENTS]");
            foreach (var enrollment in enrollments)
            {
                if (enrollment.Value.Any())
                {
                    var studentIds = string.Join(",", enrollment.Value);
                    writer.WriteLine($"{enrollment.Key}:{studentIds}");
                }
            }
            writer.WriteLine();
        }

        private void WriteGradesSection(StreamWriter writer, Dictionary<string, Dictionary<string, int>> grades)
        {
            writer.WriteLine("[GRADES]");
            foreach (var studentGrade in grades)
            {
                if (studentGrade.Value.Any())
                {
                    var gradeLines = studentGrade.Value.Select(g => $"{g.Key}={g.Value}");
                    writer.WriteLine($"{studentGrade.Key}:{string.Join(",", gradeLines)}");
                }
            }
        }
    }
}