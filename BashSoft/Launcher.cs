namespace BashSoft
{
    class Launcher
    {
        static void Main()
        {
            Tester.CompareContent(@"..\..\04-actual.txt",@"..\..\04-expected.txt");
        }
    }
}
