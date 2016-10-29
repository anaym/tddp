using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization.Statistic
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
            // CR (krait): Зачем?
            yield break;
        }

        public static IEnumerable<string> FromFile(string fileName, string codeName = null)
        {
            return File.ReadAllLines(fileName, codeName == null ? Encoding.Default : Encoding.GetEncoding(codeName));
        }

        public static IEnumerable<string> FromFolder(string pathToFolder, string availableExtension, string codeName = null)
        {
            // CR (krait): 
            // А не написать ли всё тело этой функции в одно выражение? Подсказка:
            // Directory.GetDirectories(pathToFolder, "*." + availableExtension, SearchOption.AllDirectories)

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
                catch (Exception)
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
                catch (Exception)
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