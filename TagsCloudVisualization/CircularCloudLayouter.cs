using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Vector Centre;
        private readonly List<Rectangle> rectangles;
        private readonly HashSet<Vector> spots;
        private Vector averageVector;

        public CircularCloudLayouter(Vector centre)
        {
            Centre = centre;
            rectangles = new List<Rectangle>();
            averageVector = Vector.Zero;
            spots = new HashSet<Vector>();
        }

        public CircularCloudLayouter() : this(new Vector(0, 0))
        { }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == null)
            {
                throw new ArgumentNullException(nameof(rectangleSize));
            }
            if (rectangles.Any() && rectangles.Last().Size.Height < rectangleSize.Height)
            {
                throw new ArgumentException("Rectangles must be ordered by desending sizes");
            }
            var rect = Rectangle.FromCentre(Centre, rectangleSize);
            if (rectangles.Any())
            {
                rect = TryInsert(rectangleSize);
                if (rect == null)
                {
                    throw new Exception("Can`t insert rectangle!");
                }
            }
            
            averageVector = averageVector.Add(rect.Centre.Sub(Centre));
            rectangles.Add(rect);

            spots.Add(new Vector(rect.Left, rect.Centre.Y));
            spots.Add(new Vector(rect.Right, rect.Centre.Y));
            spots.Add(new Vector(rect.Centre.X, rect.Top));
            spots.Add(new Vector(rect.Centre.X, rect.Bottom));
            spots.Add(rect.LeftBottom);
            spots.Add(rect.RightTop);
            spots.Add(rect.LeftTop);
            spots.Add(rect.RightBottom);

            return rect;
        }

        private Rectangle TryInsert(Size size)
        {
            var rect = spots
                .SelectMany(w => new []
                {
                    Rectangle.FromLeftBottom(w, size),
                    Rectangle.FromRightBottom(w, size),
                    Rectangle.FromLeftTop(w, size),
                    Rectangle.FromRightTop(w, size),
                })
                .Where(r => !IsIntersected(r)).ToList()
                .MinOrDefault(Aberration);
            return rect;
        }

        private int Aberration(Rectangle rect)
        {
            return rect.Centre.Sub(Centre).Add(averageVector).Norm;
        }

        private bool IsIntersected(Rectangle testable)
        {
            return rectangles.Any(r => r.IsIntersected(testable, false));
        }

        public IEnumerable<Rectangle> GetRectangles() => rectangles;
    }
}
