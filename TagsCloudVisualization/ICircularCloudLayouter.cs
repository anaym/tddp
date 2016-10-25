using System.Collections.Generic;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        IEnumerable<Rectangle> Rectangles { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}