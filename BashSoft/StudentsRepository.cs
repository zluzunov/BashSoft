namespace BashSoft
{
    using System;
    using System.Collections.Generic;

    public static class StudentsRepository
    {
        private const char InputSeparator = ' ';
        private const int InputCourseIndex = 0;
        private const int InputStudentIndex = 1;
        private const int InputMarkIndex = 2;

        public static bool IsDataInitialized = false;

        private static Dictionary<string, Dictionary<string, List<int>>> _studentsByCourse;

        public static void InitializeData()
        {
            if (IsDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data ...");
                _studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData();
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInicializedException);
            }
        }

        private static void ReadData()
        {
            string input = Console.ReadLine();

            while (!string.IsNullOrEmpty(input))
            {
                string[] tokens = input.Split(InputSeparator);

                string course = tokens[InputCourseIndex];
                string student = tokens[InputStudentIndex];
                int mark = int.Parse(tokens[InputMarkIndex]);

                if (!_studentsByCourse.ContainsKey(course))
                {
                    _studentsByCourse.Add(course, new Dictionary<string, List<int>>());
                }

                if (!_studentsByCourse[course].ContainsKey(student))
                {
                    _studentsByCourse[course].Add(student, new List<int>());
                }

                _studentsByCourse[course][student].Add(mark);
                input = Console.ReadLine();
            }

            IsDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }
    }
}
