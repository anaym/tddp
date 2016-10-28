using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Fclp;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    class MyClass : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }

    class Program
    {
        static MyClass Foo()
        {
            using (var c = new MyClass())
            {
                return c;
            }
        }

        static void Main(string[] args)
        {
            var c = Foo();
            Console.WriteLine("!");
            var b = c;

            var commandLineParser = new FluentCommandLineParser<TagCloudTask>();

            commandLineParser
                .Setup(options => options.FileSource)
                .As('f', "file")
                .SetDefault(null)
                .WithDescription("File source");

            commandLineParser
                .Setup(options => options.DirectorySource)
                .As('d', "directory")
                .SetDefault(null)
                .WithDescription("Directory source");

            commandLineParser
                .Setup(options => options.ExtensionFilter)
                .As('e', "extension")
                .SetDefault("")
                .WithDescription("Extension filter");

            commandLineParser
                .Setup(options => options.MinWordHeight)
                .As('i', "min")
                .SetDefault(16)
                .WithDescription("Minimal word height");

            commandLineParser
                .Setup(options => options.MaxWordWidth)
                .As('s', "max")
                .SetDefault(64)
                .WithDescription("Maximum word height");

            commandLineParser
                .Setup(options => options.Count)
                .As('c', "count")
                .SetDefault(100)
                .WithDescription("Maximum");

            commandLineParser
                .Setup(options => options.RenderBackgroundRectangles)
                .As('r')
                .SetDefault(false)
                .WithDescription("Render background rectangles");

            commandLineParser
                .Setup(options => options.OutFileName)
                .As('o', "out")
                .SetDefault("out.png")
                .WithDescription("Out image name");

            commandLineParser
                .SetupHelp("h", "help")
                .Callback(text => Console.WriteLine(text));

            if (commandLineParser.Parse(args).HelpCalled)
                return;

            var task = commandLineParser.Object;
            var data = task.CreateStatistic().ToDictionary(p => p.Key, p=> p.Value);

            var layoter = new CircularCloudLayouter(Vector.Zero, new Vector(2, 2));
            var max = data.Count != 0 ? data.Max(p => p.Value) : 1;
            var min = data.Count != 0 ? data.Min(p => p.Value) : 0;
            var tags = TagCloud.FromLimits(layoter, task.MinWordHeight, task.MaxWordWidth, max, min);
            var renderer = new TagCloudRenderer (task.RenderBackgroundRectangles);
            renderer.AddManyColors(Color.DarkBlue, Color.OrangeRed, Color.DarkGreen);
            tags.PutManyTags(data);
            renderer.RenderToBitmap(tags).Save(task.OutFileName, ImageFormat.Png);
            Process.Start(task.OutFileName);
        }
    }
}
