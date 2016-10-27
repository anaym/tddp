﻿using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Utility;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public readonly Vector Centre;
        public readonly Vector Extension;

        private readonly List<Rectangle> rectangles;
        private readonly HashSet<Vector> spots;
        private Vector averageVector;

        public CircularCloudLayouter(Vector centre, Vector extension)
        {
            Centre = centre;
            Extension = extension;
            rectangles = new List<Rectangle>();
            averageVector = Vector.Zero;
            spots = new HashSet<Vector>();
            
            if (Extension.X <= 0)
                // !CR (krait): Почти, только в nameof попало немного не то.
                throw new ArgumentException(nameof(Extension.X));
            if (Extension.Y <= 0)
                throw new ArgumentException(nameof(Extension.Y));
        }

        public CircularCloudLayouter(Vector centre) : this(centre, new Vector(2, 1))
        { }

        public CircularCloudLayouter() : this(new Vector(0, 0), new Vector(2, 1))
        { }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = Rectangle.FromCentre(Centre, rectangleSize);
            if (rectangles.Any())
            {
                rect = TryInsert(rectangleSize);
            }
            
            averageVector = averageVector + rect.Centre - Centre;
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
            var newVector = rect.Centre - Centre + averageVector;
            return Math.Abs(newVector.X / Extension.X) + Math.Abs(newVector.Y / Extension.Y);
        }

        private bool IsIntersected(Rectangle subject)
        {
            return rectangles.Any(r => r.IsIntersected(subject, false));
        }

        public IEnumerable<Rectangle> Rectangles => rectangles;
    }
}
