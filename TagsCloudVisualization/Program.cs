using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Fclp;
using Utility;
using Utility.Geometry;
using Utility.Statistic;

namespace TagsCloudVisualization
{
    class Program
    {
        private static void Main(string[] args)
        {
            var commandLineParser = PrepareCommandLineParser();

            if (commandLineParser.Parse(args).HelpCalled)
                return;

            var task = commandLineParser.Object;
            var data = Statistic.FromTask(task);
            if (task.XmlSource == null) data.ToXml(new StreamWriter(File.OpenWrite("stat.xml")));

            var layoter = new CircularCloudLayouter(Vector.Zero, new Vector((int)(task.Ratio * 10), (int)(10 / task.Ratio)));
            var max = data.Count != 0 ? data.Max(p => p.Value) : 1;
            var min = data.Count != 0 ? data.Min(p => p.Value) : 0;
            if (data.Count == 0) Console.WriteLine("Data is not founded");
            var tags = TagCloud.FromLimits(layoter, task.MinWordHeight, task.MaxWordWidth, max, min);
            var renderer = new TagCloudRenderer(task.RenderBackgroundRectangles);
            renderer.AddManyColors(Color.DarkBlue, Color.OrangeRed, Color.DarkGreen);
            tags.PutManyTags(data);
            renderer.RenderToBitmap(tags).Save(task.OutFileName, ImageFormat.Png);
            Process.Start(task.OutFileName);
        }

        private static FluentCommandLineParser<TagCloudTask> PrepareCommandLineParser()
        {
            var commandLineParser = new FluentCommandLineParser<TagCloudTask>();
            
            commandLineParser
                .Setup(options => options.XmlSource)
                .As('x', "xml")
                .SetDefault(null)
                .WithDescription("XML source");

            commandLineParser
                .Setup(options => options.FileSource)
                .As('f', "file")
                .SetDefault(null)
                .WithDescription("File source");

            commandLineParser
                .Setup(options => options.CodePage)
                .As('e', "encoding")
                .SetDefault(null)
                .WithDescription("Encoding from read");

            commandLineParser
                .Setup(options => options.FolderSource)
                .As('d', "directory")
                .SetDefault(null)
                .WithDescription("Directory source");

            commandLineParser
                .Setup(options => options.AvaibleTypes)
                .As('t', "type")
                .SetDefault("")
                .WithDescription("Type file filter");

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
                .Setup(options => options.OnlyLetters)
                .As("ol")
                .SetDefault(false)
                .WithDescription("Include only letters");

            commandLineParser
                .Setup(options => options.MinWordLength)
                .As('l')
                .SetDefault(0)
                .WithDescription("Include words longer, that num");

            commandLineParser
                .Setup(options => options.Ratio)
                .As('r', "ratio")
                .SetDefault(1.5)
                .WithDescription("Ratio width per height");

            commandLineParser
                .Setup(options => options.Count)
                .As('c', "count")
                .SetDefault(100)
                .WithDescription("Count of processing words");

            commandLineParser
                .Setup(options => options.RenderBackgroundRectangles)
                .As('b')
                .SetDefault(false)
                .WithDescription("Show background rectangles");

            commandLineParser
                .Setup(options => options.OutFileName)
                .As('o', "out")
                .SetDefault($"out_{DateTime.Now.Millisecond}.png")
                .WithDescription("Out image name");

            commandLineParser
                .SetupHelp("h", "help")
                .Callback(text => Console.WriteLine(text));

            return commandLineParser;
        }
    }
}
