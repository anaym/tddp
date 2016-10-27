using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Statistic;

namespace TagsCloudVisualization
{
    public class TagCloudTask
    {
        public string FileSource { get; set; }
        public string DirectorySource { get; set; }
        public string ExtensionFilter { get; set; }
        public int MinWordHeight { get; set; }
        public int MaxWordWidth { get; set; }
        public string OutFileName { get; set; }
        public int Count { get; set; }
        public bool RenderBackgroundRectangles { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CreateStatistic()
        {
            var src = Enumerable.Empty<string>();
            if (DirectorySource != null) src = GetLines.FromFolder(DirectorySource, ExtensionFilter);
            else if (FileSource != null) src = GetLines.FromFile(FileSource);
            else src = GetLines.FromInputStream();
            return src
                .ExtractWords(true)
                .CreateStatistic(Count);
        }
    }
}