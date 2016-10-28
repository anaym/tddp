using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            yield break;
        }

        public static IEnumerable<string> FromFile(string fileName)
        {
            return File.ReadAllLines(fileName);
        }

        public static IEnumerable<string> FromFolder(string pathToFolder, string avaibleExtension)
        {
            var now = new DirectoryInfo(pathToFolder);
            var mustVisit = new Queue<DirectoryInfo>(new[] { now });
            var files = new HashSet<string>();
            while (mustVisit.Count != 0)
            {
                now = mustVisit.Dequeue();
                try
                {
                    foreach (var folder in now.GetDirectories()) mustVisit.Enqueue(folder);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Can not read directory: {now.FullName}");
                }
                try
                {
                    foreach (var file in now.GetFiles().Where(f => f.Name.EndsWith(avaibleExtension)))
                    {
                        files.Add(file.FullName);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Can not read directory: {now.FullName}");
                }
            }
            foreach (var file in files)
            {
                foreach (var line in FromFile(file))
                {
                    yield return line;
                }
            }
        }
    }
}