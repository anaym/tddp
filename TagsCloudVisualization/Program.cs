using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Geometry;

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

        static void Main(string[] args)
        {
            var charHeightPerWidth = 1.35;
            var minWidth = 16;
            var maxWidth = 64;
            var outFileName = "ot.png";

            ShporaStatistic = ShporaStatistic
                .Select(p => new KeyValuePair<string, int>("[" + p.Key.Trim() + "]", p.Value))
                .ToDictionary(p => p.Key, p => p.Value);/*
            var cloud = new TagCloudToBitmapConverter(ShporaStatistic, new Size(minWidth, (int) (minWidth*charHeightPerWidth)),
                new Size(maxWidth, (int) (maxWidth*charHeightPerWidth)));
            var bitmap = cloud.ToBitmap();*/
            var renderer = new CloudRenderer(new CircularCloudLayouter(Vector.Zero, new Vector(3, 1)));
            renderer.PutTags(ShporaStatistic); //а если повторно вызвать PutTags?
            var size = renderer.VisualizeRectangle.Size;
            var bitmap = new Bitmap(size.Width, size.Height);
            renderer.Render(Graphics.FromImage(bitmap));
            bitmap.Save(outFileName, ImageFormat.Png);
            Process.Start(outFileName);
        }


    }
}
