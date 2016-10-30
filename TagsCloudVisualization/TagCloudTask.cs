using System.Collections.Generic;
using Utility.Statistic;

namespace TagsCloudVisualization
{
    public class TagCloudTask : StatisticTask
    {
        public int MinWordHeight { get; set; }
        public int MaxWordWidth { get; set; }
        public string OutFileName { get; set; }
        public bool RenderBackgroundRectangles { get; set; }
        public double Ratio { get; set; }
        //public StatisticTask StatisticTask { get; set; } = new StatisticTask();
    }
}