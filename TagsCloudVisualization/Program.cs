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
            var cloud = new TagCloud(stat2, new Size(16, 32), new Size(128, 256));
            var bitmap = cloud.ToBitmap(false);
            bitmap.Save("out.bmp", ImageFormat.Bmp);
        }
    }
}
