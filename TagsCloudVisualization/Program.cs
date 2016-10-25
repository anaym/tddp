using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    // CR (krait): 
    // В коде много неудачных названий, которые вводят читателя в заблуждение. Некоторые из них я поправил сам, посмотри на такие места и попробуй понять, что было не так.

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

        static void Main(string[] args)
        {
            var charHeightPerWidth = 2;
            var minWidth = 32;
            var outFileName = "out.png";

            ShporaStatistic = ShporaStatistic
                .Select(p => new KeyValuePair<string, int>($"[{p.Key.Trim()}]", p.Value))
                .ToDictionary(p => p.Key, p => p.Value);

            var rnd = new Random();
            var bigData = Enumerable.Range(0, 100).ToDictionary(i => i + "_" + rnd.Next()%256);

            var layoter = new CircularCloudLayouter(Vector.Zero, new Vector(2, 1));
            layoter.PutNextRectangle(new Size(200, 100));

            var tags = new TagCloud(
                layoter,
                new Size(minWidth, (int) (minWidth*charHeightPerWidth)),
                v => v*(int)Math.Sqrt(v));

            var renderer = new TagCloudRenderer ();
            tags.PutNextTag("{SMALL}", 0);
            tags.PutManyTags(bigData);
            renderer.RenderToBitmap(tags).Save(outFileName, ImageFormat.Png);
            Process.Start(outFileName);
        }
    }
}
