namespace BashSoft
{
    using System.Collections.Generic;
    using System.IO;

    public static class IOManager
    {
        private const char Separator = '\\';

        public static void TraverseDirectory(string path)
        {
            OutputWriter.WriteEmptyLine();
            int initialIdentation = path.Split(Separator).Length;
            Queue<string> subfolders = new Queue<string>();
            subfolders.Enqueue(path);

            while (subfolders.Count > 0)
            {
                string currentPath = subfolders.Dequeue();
                int identation = currentPath.Split(Separator).Length - initialIdentation;

                string identationString = new string('-', identation);
                string directoryString = $"{identationString}{currentPath}";
                OutputWriter.WriteMessageOnNewLine(directoryString);

                foreach (string directoryPath in Directory.GetDirectories(currentPath))
                {
                    subfolders.Enqueue(directoryPath);
                }
            }
        }
    }
}
