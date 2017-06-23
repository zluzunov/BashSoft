namespace BashSoft
{
    using System;
    using System.IO;

    public static class Tester
    {
        private const string MismatchesFileName = @"\Mismatches.txt";
        private const char PathSeparator = '\\';
        private const string FilesAreIdenticalMessage = "Files are identical. There are no mismatches.";
        private const string ReadingFilesMessage = "Reading files...";
        private const string FilesReadMessage = "Files read!";
        private const string ComparingFilesMessage = "Comparing files...";

        public static void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine(ReadingFilesMessage);

            string mismatchPath = GetMismatchPath(expectedOutputPath);

            string[] actualOutputLines = File.ReadAllLines(userOutputPath);
            string[] expectedOutputLines = File.ReadAllLines(expectedOutputPath);

            bool hasMismatch;
            string[] mismatches = GetLineWithPossibleMismatches(actualOutputLines, expectedOutputLines, out hasMismatch);

            PrintOutput(mismatches, hasMismatch, mismatchPath);
            var filesRead = FilesReadMessage;
            OutputWriter.WriteMessageOnNewLine(filesRead);
        }

        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchPath, mismatches);
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(FilesAreIdenticalMessage);
            }
        }

        private static string[] GetLineWithPossibleMismatches(
            string[] actualOutputLines, 
            string[] expectedOutputLines, 
            out bool hasMismatch
            )
        {
            int minOutputLines = actualOutputLines.Length;
            if (actualOutputLines.Length != expectedOutputLines.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }
            hasMismatch = false;
            string output;
            string[] mismatches = new string[actualOutputLines.Length];
            OutputWriter.WriteMessageOnNewLine(ComparingFilesMessage);

            for (int i = 0; i < minOutputLines; i++)
            {
                string actualLine = actualOutputLines[i];
                string expectedLine = expectedOutputLines[i];

                if (!actualLine.Equals(expectedLine))
                {
                    output = $"Mismatch at line {i} -- expected: \"{expectedLine}\", actual: \"{actualLine}\"";
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }
                mismatches[i] = output;
            }
            return mismatches;
        }

        private static string GetMismatchPath(string expectedOutputPath)
        {
            int indexOf = expectedOutputPath.LastIndexOf(PathSeparator);
            string direcotryPath = expectedOutputPath.Substring(0, indexOf);
            string finalPath = direcotryPath + MismatchesFileName;
            return finalPath;
        }
    }
}
