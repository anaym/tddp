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
            {"	Котельников Алексей	",  71 }   ,
{"	Сивухин Никита	",  67 }   ,
{"	Акулин Максим	",  62 }   ,
{"	Борзов Артем	",  61 }   ,
{"	Яркеев Станислав	",  59 }   ,
{"	Тимерханов Константин	",  58 }   ,
{"	Трофимов Павел	",  57 }   ,
{"	Нартов Никита	",  56 }   ,
{"	Толстов Антон	",  53 }   ,
{"	Бикташев Галлям	",  52 }   ,
{"	Шестаков Алексей	",  49 }   ,
{"	Неволин Роман	",  46 }   ,
{"	Плисковский Михаил	",  42 }   ,
{"	Пешков Евгений	",  41 }   ,
{"	Дубровин Алексей	",  31 }   ,
{"	Смирнов Иван	",  30 }   ,
{"	Ляпустин Максим	",  30 }   ,
{"	Хапов Кирилл	",  25 }   ,
{"	Белев Александр	",  25 }   ,
{"	Лысов Дмитрий	",  22 }   ,
{"	Рябинин Сергей	",  20 }   ,
{"	Нужин Егор	",  18 }   ,
{"	Федянин Станислав	",  18 }   ,
{"	Насиров Руслан	",  14 }   ,
{"	Сатов Александр	",  12 }   ,
{"	Кавешников Денис	",  11 }   ,
{"	Рыжкин Артем	",  10 }   ,
{"	Лозинский Степан	",  10 }   ,
{"	Самородов Алексей	",  9  }   ,
{"	Кошара Павел	",  7  }   ,
{"	Головин Евгений	",  4  }   ,
{"	Карманов Кирилл	",  1  }   ,
{"	Нагаев Дмитрий	",  0  }   ,
{"	Аменд Мария	",  0  }   ,
{"	Ватолин Алексей	",  0  }   ,
{"	Захаров Алексей	",  0  }   ,
{"	Лукшто Дмитрий	",  0  }   ,
{"	Михаил Вострецов	",  0  }   ,

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
            var stat2 = new Dictionary<string, int>();
            var rnd = new Random(123);
            for (int i = 1; i < 500; i++)
            {
                stat2.Add(i.ToString() + "_" +  rnd.Next()%(Math.Abs(rnd.Next()%100) + 1), i);
            }
            var cloud = new TagCloud(shpora, new Size(16, 32), new Size(128, 256));
            var bitmap = cloud.ToBitmap(false);
            bitmap.Save("out.bmp", ImageFormat.Png);
        }
    }
}
