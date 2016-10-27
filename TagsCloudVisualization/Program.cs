using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Geometry;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    class Program
    {
        #region Data

        public static Dictionary<string, int> ShporaStatistic = new Dictionary<string, int>
        {
            {"	Котельников Алексей	", 71},
            {"	Сивухин Никита	", 67},
            {"	Акулин Максим	", 62},
            {"	Борзов Артем	", 61},
            {"	Яркеев Станислав	", 59},
            {"	Тимерханов Константин	", 58},
            {"	Трофимов Павел	", 57},
            {"	Нартов Никита	", 56},
            {"	Толстов Антон	", 53},
            {"	Бикташев Галлям	", 52},
            {"	Шестаков Алексей	", 49},
            {"	Неволин Роман	", 46},
            {"	Плисковский Михаил	", 42},
            {"	Пешков Евгений	", 41},
            {"	Дубровин Алексей	", 31},
            {"	Смирнов Иван	", 30},
            {"	Ляпустин Максим	", 30},
            {"	Хапов Кирилл	", 25},
            {"	Белев Александр	", 25},
            {"	Лысов Дмитрий	", 22},
            {"	Рябинин Сергей	", 20},
            {"	Нужин Егор	", 18},
            {"	Федянин Станислав	", 18},
            {"	Насиров Руслан	", 14},
            {"	Сатов Александр	", 12},
            {"	Кавешников Денис	", 11},
            {"	Рыжкин Артем	", 10},
            {"	Лозинский Степан	", 10},
            {"	Самородов Алексей	", 9},
            {"	Кошара Павел	", 7},
            {"	Головин Евгений	", 4},
            {"	Карманов Кирилл	", 1},
            {"	Нагаев Дмитрий	", 0},
            {"	Аменд Мария	", 0},
            {"	Ватолин Алексей	", 0},
            {"	Захаров Алексей	", 0},
            {"	Лукшто Дмитрий	", 0},
            {"	Михаил Вострецов	", 0},

        };

        public static Dictionary<string, int> SimpleStatistic = new Dictionary<string, int>
        {
            {"one", 1},
            {"two", 2},
            {"two2", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"six2", 6},
            {"seven", 7},
            {"vosem", 8},
            {"vosem2", 8},
            {"vosem3", 8},
            {"nine", 9},
            {"ten", 10},
            {"11", 11},
            {"12", 12},
            {"13", 13},
        };

        #endregion


        public static Dictionary<string, int> LoadFromDir(string pathToDir, string extension, int count = 100,
            bool onlyLetters = false)
        {
            var now = new DirectoryInfo(pathToDir);
            var files = new List<FileInfo>();
            var mustVisit = new Queue<DirectoryInfo>(new[] {now});
            while (mustVisit.Count != 0)
            {
                now = mustVisit.Dequeue();
                try
                {
                    foreach (var folder in now.GetDirectories()) mustVisit.Enqueue(folder);
                }
                catch (Exception)
                {
                }
                try
                {
                    files.AddRange(now.GetFiles());
                }
                catch (Exception)
                {
                }
            }
            var stat = new Dictionary<string, int>();
            foreach (var file in files.Where(f => f.Name.EndsWith(extension)))
            {
                try
                {
                    LoadData(file, stat, onlyLetters: onlyLetters);

                }
                catch (Exception)
                { }
            }
            return stat
                .OrderByDescending(p => p.Value)
                .Take(count)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public static Dictionary<string, int> LoadData(FileInfo file, Dictionary<string, int> begin = null, int? count = null, bool onlyLetters = false)
        {
            var separators = new[]
            {
                '.', ',', '!', '?', ':', ';',
                '[', ']', '{', '}', '(', ')', '<', '>',
                '-', '_', '=', '+', '^',
                '@', '#', '$', '%', '^', '&', '*',
                '\"', '\'', '~', '`',
                '\\', '|', '/',
                '\t', ' ', '\r', '\n'
            };
            var words = File.ReadAllLines(file.FullName)
                .SelectMany(l => l.Split(separators, StringSplitOptions.RemoveEmptyEntries));
            var stat = begin ?? new Dictionary<string, int>();
            if (onlyLetters) words = words.Where(w => w.All(char.IsLetter));
            foreach (var word in words)
            {
                if (!stat.ContainsKey(word)) stat[word] = 0;
                stat[word]++;
            }
            if (count == null)
                return stat;
            return stat
                .OrderByDescending(p => p.Value)
                .Take(count.Value)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        static void Main(string[] args)
        {
            var outFileName = "out.png";

            ShporaStatistic = ShporaStatistic
                .Select(p => new KeyValuePair<string, int>($"[{p.Key.Trim()}]", p.Value))
                .ToDictionary(p => p.Key, p => p.Value);

            var rnd = new Random();
            var bigData = Enumerable.Range(0, 100).ToDictionary(i => i + "_" + rnd.Next()%256);

            var layoter = new CircularCloudLayouter(Vector.Zero, new Vector(3, 2));
            //layoter.PutNextRectangle(new Size(200, 100));

            var data = LoadFromDir(@"C:\Users\Anton Tolstov\GitHub\sed\tddp", "cs", onlyLetters:true);

            var tags = TagCloud.AsMapping(layoter, 32, 256, data.Max(p => p.Value), data.Min(p => p.Value));

            var renderer = new TagCloudRenderer ();
            renderer.AddManyColor(Color.DarkBlue, Color.OrangeRed, Color.DarkGreen);
            //tags.PutNextTag("{SMALL}", 0);
            tags.PutManyTags(data);
            renderer.RenderToBitmap(tags).Save(outFileName, ImageFormat.Png);
            Process.Start(outFileName);
        }
    }
}
