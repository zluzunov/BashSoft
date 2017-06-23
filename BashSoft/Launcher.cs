namespace BashSoft
{
    class Launcher
    {
        static void Main()
        {
            IOManager.ChangeCurrentDirectoryAbsolute(@"C:\Windows");
            IOManager.TraverseDirectory(20);
        }
    }
}
