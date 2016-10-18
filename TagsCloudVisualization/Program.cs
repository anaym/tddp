using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    class Program
    {
        public static Dictionary<string, int> shpora = new Dictionary<string, int>
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

        static void Main(string[] args)
        {
            var stat = new Dictionary<string, int>
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
            var c = 0;
            var tr = new Transliter.Form1();
            shpora = shpora
                .Select(p => new KeyValuePair<string, int>("[" + p.Key.Trim() + "]", p.Value))
                .ToDictionary(p => p.Key, p => p.Value);
            var stat2 = new Dictionary<string, int>();
            var rnd = new Random(123);
            for (int i = 1; i < 100; i++)
            {
                stat2.Add(i.ToString() + "_" + rnd.Next()%(Math.Abs(rnd.Next()%100) + 1), i);
            }
            var hpw = 1.35;
            var minw = 16;
            var maxw = 64;
            var cloud = new TagCloud(stat2, new Size(minw, (int) (minw*hpw)), new Size(maxw, (int) (maxw*hpw)));
            var bitmap = cloud.ToBitmap(false);
            bitmap.Save("out.bmp", ImageFormat.Png);
        }


    }
}

namespace Transliter
{
    public partial class Form1
    {
        Dictionary<string, string> words = new Dictionary<string, string>();

        public Form1()
        {
            words.Add("а", "a");
            words.Add("б", "b");
            words.Add("в", "v");
            words.Add("г", "g");
            words.Add("д", "d");
            words.Add("е", "e");
            words.Add("ё", "yo");
            words.Add("ж", "zh");
            words.Add("з", "z");
            words.Add("и", "i");
            words.Add("й", "j");
            words.Add("к", "k");
            words.Add("л", "l");
            words.Add("м", "m");
            words.Add("н", "n");
            words.Add("о", "o");
            words.Add("п", "p");
            words.Add("р", "r");
            words.Add("с", "s");
            words.Add("т", "t");
            words.Add("у", "u");
            words.Add("ф", "f");
            words.Add("х", "h");
            words.Add("ц", "c");
            words.Add("ч", "ch");
            words.Add("ш", "sh");
            words.Add("щ", "sch");
            words.Add("ъ", "j");
            words.Add("ы", "i");
            words.Add("ь", "j");
            words.Add("э", "e");
            words.Add("ю", "yu");
            words.Add("я", "ya");
            words.Add("А", "A");
            words.Add("Б", "B");
            words.Add("В", "V");
            words.Add("Г", "G");
            words.Add("Д", "D");
            words.Add("Е", "E");
            words.Add("Ё", "Yo");
            words.Add("Ж", "Zh");
            words.Add("З", "Z");
            words.Add("И", "I");
            words.Add("Й", "J");
            words.Add("К", "K");
            words.Add("Л", "L");
            words.Add("М", "M");
            words.Add("Н", "N");
            words.Add("О", "O");
            words.Add("П", "P");
            words.Add("Р", "R");
            words.Add("С", "S");
            words.Add("Т", "T");
            words.Add("У", "U");
            words.Add("Ф", "F");
            words.Add("Х", "H");
            words.Add("Ц", "C");
            words.Add("Ч", "Ch");
            words.Add("Ш", "Sh");
            words.Add("Щ", "Sch");
            words.Add("Ъ", "J");
            words.Add("Ы", "I");
            words.Add("Ь", "J");
            words.Add("Э", "E");
            words.Add("Ю", "Yu");
            words.Add("Я", "Ya");
        }

        public string Translit(string str)
        {
            var now = str;
            foreach (KeyValuePair<string, string> pair in words)
            {
                now = now.Replace(pair.Key, pair.Value);
            }
            return now;
        }
    }
}
