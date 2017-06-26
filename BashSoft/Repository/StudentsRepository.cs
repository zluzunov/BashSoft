namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class StudentsRepository
    {
        private const char InputSeparator = ' ';
        private const string DirectorySeparator = "\\";
        private const string RegExGroupNameCourse = "course";
        private const string RegExGroupNameStudent = "student";
        private const string RegExGroupNameMark = "mark";

        public static bool IsDataInitialized = false;

        private static Dictionary<string, Dictionary<string, List<int>>> _studentsByCourse;

        public static void InitializeData(string inputFile)
        {
            if (!IsDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data ...");
                _studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(inputFile);
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInicializedException);
            }
        }

        private static void ReadData(string fileName)
        {
            string path = SessionData.CurrentPath + DirectorySeparator + fileName;

            if (File.Exists(path))
            {
                string pattern = @"(?<" + RegExGroupNameCourse
                    + @">[A-Z][A-Za-z+#]*_[A-Z][a-z]{2}_\d{4})\s+(?<" + RegExGroupNameStudent
                    + @">[A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(?<" + RegExGroupNameMark
                    + @">\d+)";
                Regex rgx = new Regex(pattern);
                string[] allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && rgx.IsMatch(allInputLines[line]))
                    {
                        Match currentMatch = rgx.Match(allInputLines[line]);
                        string courseName = currentMatch.Groups[RegExGroupNameCourse].Value;
                        string username = currentMatch.Groups[RegExGroupNameStudent].Value;
                        int studentScoreOnTask;
                        bool hasParsedScore = int.TryParse(currentMatch.Groups[RegExGroupNameMark].Value, out studentScoreOnTask);

                        if (hasParsedScore && studentScoreOnTask >= 0 && studentScoreOnTask <= 100)
                        {
                            if (!_studentsByCourse.ContainsKey(courseName))
                            {
                                _studentsByCourse.Add(courseName, new Dictionary<string, List<int>>());
                            }

                            if (!_studentsByCourse[courseName].ContainsKey(username))
                            {
                                _studentsByCourse[courseName].Add(username, new List<int>());
                            }

                            _studentsByCourse[courseName][username].Add(studentScoreOnTask);
                        }
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }

            IsDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private static bool IsQueryForCoursePossible(string courseName)
        {
            if (_studentsByCourse.ContainsKey(courseName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
            }

            return true;
        }

        private static bool IsQueryForStudentPossiblе(string courseName, string studentUserName)
        {
            if (IsQueryForCoursePossible(courseName) && _studentsByCourse[courseName].ContainsKey(studentUserName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }

            return false;
        }

        public static void GetStudentScoresFromCourse(string courseName, string username)
        {
            if (IsQueryForStudentPossiblе(courseName, username))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, List<int>>(username, _studentsByCourse[courseName][username]));
            }
        }



        public static void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}");
                foreach (var studentMarksEntry in _studentsByCourse[courseName])
                {
                    OutputWriter.PrintStudent(studentMarksEntry);
                }
            }
        }

        public static void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = _studentsByCourse[courseName].Count;
                }

                RepositoryFilters.FilterAndTake(_studentsByCourse[courseName], givenFilter, studentsToTake.Value);
            }
        }

        public static void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = _studentsByCourse[courseName].Count;
                }

                RepositorySorters.OrderAndTake(_studentsByCourse[courseName], comparison, studentsToTake.Value);
            }
        }
    }
}
