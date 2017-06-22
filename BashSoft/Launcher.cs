namespace BashSoft
{
    class Launcher
    {
        static void Main()
        {
            StudentsRepository.InitializeData();
            StudentsRepository.GetStudentScoresFromCourse("Unity", "Ivan");
        }
    }
}
