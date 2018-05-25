using System;
using System.Collections.Generic;
using System.IO;

namespace HorseSpeed.Runner.Logic
{
    internal class FileWriter
    {
        private readonly string _outDirectory;

        public FileWriter(string outDirectory)
        {
            _outDirectory = outDirectory;
        }

        public void WriteTimingFile(IReadOnlyList<int> orderedList, string filePath)
        {
            Directory.CreateDirectory(_outDirectory);
            File.WriteAllText(filePath, string.Join(Environment.NewLine, orderedList));
        }

        public string GenerateOutFilePath(string fromClause)
        {
            var startIndex = fromClause.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            var length = fromClause.Length - startIndex;
            var extensionIndex = fromClause.LastIndexOf(".", StringComparison.Ordinal);

            if (extensionIndex > startIndex)
            {
                length = extensionIndex - startIndex;
            }

            var fileName = fromClause.Substring(startIndex, length).Replace("*", "star") + "_" + Guid.NewGuid() + ".csv";

            return Path.Combine(_outDirectory, fileName);
        }
    }
}