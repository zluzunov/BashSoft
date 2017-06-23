namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class IOManager
    {
        private const char Separator = '\\';
        private const string UpperDirecotryString = "..";
        private const char AlignCharacter = '-';

        public static void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            int initialIdentation = SessionData.CurrentPath.Split(Separator).Length;
            Queue<string> subfolders = new Queue<string>();
            subfolders.Enqueue(SessionData.CurrentPath);

            while (subfolders.Count > 0)
            {
                string currentPath = subfolders.Dequeue();
                int identation = currentPath.Split(Separator).Length - initialIdentation;

                if (depth - identation < 0)
                {
                    break;
                }

                OutputWriter.WriteMessageOnNewLine($"{new string(AlignCharacter, identation)}{currentPath}");

                try
                {
                    foreach (var file in Directory.GetFiles(currentPath))
                    {
                        int indexOfLastSlash = file.LastIndexOf(Separator);
                        string fileName = file.Substring(indexOfLastSlash);
                        OutputWriter.WriteMessageOnNewLine(new string(AlignCharacter, indexOfLastSlash) + fileName);
                    }

                    string identationString = new string(AlignCharacter, identation);
                    string directoryString = $"{identationString}{currentPath}";
                    OutputWriter.WriteMessageOnNewLine(directoryString);

                    foreach (string directoryPath in Directory.GetDirectories(currentPath))
                    {
                        subfolders.Enqueue(directoryPath);
                    }

                }
                catch (UnauthorizedAccessException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessExceptionMessage);
                }
            }
        }

        public static void CreateDirectoryInCurrentFolder(string name)
        {
            string path = SessionData.CurrentPath + Separator + name;

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                OutputWriter.DisplayException(ExceptionMessages.ForbiddenSymbolsContaineInName);
            }

            
        }

        public static void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == UpperDirecotryString)
            {
                try
                {
                    string currentPath = SessionData.CurrentPath;
                    int indexOfLastSlash = currentPath.LastIndexOf(Separator);
                    string newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.CurrentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToGoHigherInPartitionHierarchy);
                }

                
            }
            else
            {
                string currentPath = SessionData.CurrentPath;
                currentPath += Separator + relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public static void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                return;
            }

            SessionData.CurrentPath = absolutePath;
        }
    }
}
