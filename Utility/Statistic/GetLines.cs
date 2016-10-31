using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility.Statistic
{
    public static class GetLines
    {
        public static IEnumerable<string> FromInputStream()
        {
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static IEnumerable<string> FromFile(string fileName, string codeName = null)
        {
            return File.ReadAllLines(fileName, codeName == null ? Encoding.Default : Encoding.GetEncoding(codeName));
        }

        public static IEnumerable<string> FromFolder(string pathToFolder, string availableExtension, string codeName = null)
        {
            var current = new DirectoryInfo(pathToFolder);
            var toVisit = new Queue<DirectoryInfo>(new[] { current });
            var files = new HashSet<string>();
            while (toVisit.Count != 0)
            {
                current = toVisit.Dequeue();
                try
                {
                    foreach (var folder in current.GetDirectories()) toVisit.Enqueue(folder);
                }
                catch
                {
                    Console.WriteLine($"Can not read directory: {current.FullName}");
                }
                try
                {
                    foreach (var file in current.GetFiles().Where(f => f.Name.EndsWith(availableExtension)))
                    {
                        files.Add(file.FullName);
                    }
                }
                catch
                {
                    Console.WriteLine($"Can not read directory: {current.FullName}");
                }
            }
            foreach (var file in files)
            {
                foreach (var line in FromFile(file, codeName))
                {
                    yield return line;
                }
            }
        }
    }
}