namespace BashSoft
{
    using System.Collections.Generic;

    public static class StudentsRepository
    {
        public static bool IsDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> _studentsByCourse;
    }
}
