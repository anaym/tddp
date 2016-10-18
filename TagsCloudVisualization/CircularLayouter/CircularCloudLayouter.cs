using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.CircularLayouter
{
    public class CircularCloudLayouter
    {
        public readonly Vector Centre;
        public List<Rectangle> rectangles;
        //private HashSet<Vector> insertSpots;
        private CircularCloudLayouterBucketController spots;

        public CircularCloudLayouter(Vector centre)
        {
            this.Centre = centre;
            //insertSpots = new HashSet<Vector>();
            rectangles = new List<Rectangle>();

            spots = new CircularCloudLayouterBucketController(centre);
        }

        public CircularCloudLayouter() : this(new Vector(0, 0))
        {
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Any() && rectangles.Last().Size.Height < rectangleSize.Height)
            {
                throw new ArgumentException("Rectangles must be ordered by desending sizes");
            }
            var rect = new Rectangle(rectangleSize, Centre);
            if (rectangles.Any())
            {
                var lb = TryInsertLeftBottom(rectangleSize);
                var rt = TryInsertRightTop(rectangleSize);
                if (lb == null)
                {
                    rect = rt.Item1;
                }
                else if (rt == null)
                {
                    rect = lb.Item1;
                }
                else
                {
                    rect = lb.Item2 < rt.Item2 ? lb.Item1 : rt.Item1;
                }
            }
            else
            {
                spots.AddMany(new Vector(rect.Left, Centre.Y),
                    new Vector(rect.Right, Centre.Y),
                    new Vector(Centre.X, rect.Up),
                    new Vector(Centre.X, rect.Down));
            }
            spots.Swap();
            rectangles.Add(rect);
            spots.AddMany(rect.LeftBottom, rect.RightUp, rect.LeftUp, rect.RightBottom);
            return rect;
        }

        private Tuple<Rectangle, int> TryInsertLeftBottom(Size size)
        {
            var rect = spots.Data
                .Select(w => Rectangle.FromLeftBottom(w, size))
                .Where(r => !IsIntersected(r)).ToList()
                .MinOrDefault(r => r.Centre.DistanceTo(Centre));
            if (rect == null)
                return null;
            return Tuple.Create(rect, rect.Centre.DistanceTo(Centre));
        }

        private Tuple<Rectangle, int> TryInsertRightTop(Size size)
        {
            var rect = spots.Data
                .Select(w => Rectangle.FromRightTop(w, size))
                .Where(r => !IsIntersected(r))
                .MinOrDefault(r => r.Centre.DistanceTo(Centre));
            if (rect == null)
                return null;
            return Tuple.Create(rect, rect.Centre.DistanceTo(Centre));
        }

        private bool IsIntersected(Rectangle testable)
        {
            return rectangles.Any(r => r.IsIntersected(testable, false));
        }

        public IEnumerable<Rectangle> GetRectangles() => rectangles;
    }
}
