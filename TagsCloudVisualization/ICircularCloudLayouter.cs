using System.Collections.Generic;
using Utility.Geometry;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        IEnumerable<Rectangle> Rectangles { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}