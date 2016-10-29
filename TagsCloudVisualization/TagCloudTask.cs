using System.Collections.Generic;
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
        public double Ratio { get; set; }
        public string Encoding { get; set; }
        public int MinWordLength { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CreateStatistic()
        {
            IEnumerable<string> src;
            if (DirectorySource != null) src = GetLines.FromFolder(DirectorySource, ExtensionFilter, Encoding);
            else if (FileSource != null) src = GetLines.FromFile(FileSource, Encoding);
            else src = GetLines.FromInputStream();
            return src
                .ExtractWords(true)
                .CreateStatistic(Count, MinWordLength);
        }
    }
}