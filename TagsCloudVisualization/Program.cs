using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Fclp;
using TagsCloudVisualization.Geometry;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            

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

            var layoter = new CircularCloudLayouter(Vector.Zero, new Vector(3, 2));
            var tags = TagCloud.AsMapping(layoter, task.MinWordHeight, task.MaxWordWidth, data.Max(p => p.Value), data.Min(p => p.Value));
            var renderer = new TagCloudRenderer (task.RenderBackgroundRectangles);
            renderer.AddManyColor(Color.DarkBlue, Color.OrangeRed, Color.DarkGreen);
            tags.PutManyTags(data);
            renderer.RenderToBitmap(tags).Save(task.OutFileName, ImageFormat.Png);
            Process.Start(task.OutFileName);
        }
    }
}
