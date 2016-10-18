﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.CircularLayouter
{
    public class CircularCloudLayouter
    {
        public readonly Vector Centre;
        public List<Rectangle> rectangles;
        private List<Vector> insertSpots;

        public CircularCloudLayouter(Vector centre)
        {
            this.Centre = centre;
            insertSpots = new List<Vector>();
            rectangles = new List<Rectangle>();
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
                    return rt.Item1;
                }
                rect = lb.Item2 < rt.Item2 ? lb.Item1 : rt.Item1;
            }
            try
            {
                var i = rect.IsIntersected(rectangles[3], false);
            }
            catch (Exception)
            { }
            rectangles.Add(rect);
            insertSpots.AddRange(new []{ rect.LeftBottom, rect.RightUp, rect.LeftUp, rect.RightBottom });
            return rect;
        }

        private Tuple<Rectangle, int> TryInsertLeftBottom(Size size)
        {
            var rects = insertSpots
                .Select(w => Rectangle.FromLeftBottom(w, size))
                .Where(r => !IsIntersected(r)).ToList();

            var rect = rects.MinOrDefault(r => r.Centre.DistanceTo(Centre));
            if (rect == null)
                return null;
            return Tuple.Create(rect, rect.Centre.DistanceTo(Centre));
        }

        private Tuple<Rectangle, int> TryInsertRightTop(Size size)
        {
            var rect = insertSpots
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
