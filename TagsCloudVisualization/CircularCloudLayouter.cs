using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Vector Centre;
        public List<Rectangle> rectangles;

        public CircularCloudLayouter(Vector centre)
        {
            this.Centre = centre;
            rectangles = new List<Rectangle>();
        }

        public CircularCloudLayouter() : this(new Vector(0, 0))
        { }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Any() &&
                (rectangles.Last().Size.Width < rectangleSize.Width ||
                 rectangles.Last().Size.Height < rectangleSize.Height))
            {
                throw new ArgumentException("Rectangles must be ordered by desending sizes");
            }
            var rect = new Rectangle(rectangleSize, Centre); ;
            if (rectangles.Any())
            {
                rect.LeftTop = rectangles.Last().RightBottom;
            }
            rectangles.Add(rect);
            return rect;
        }

        public IEnumerable<Rectangle> GetRectangles() => rectangles;
    }
}
