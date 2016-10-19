using System.Collections.Generic;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        IEnumerable<Rectangle> GetRectangles();
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}