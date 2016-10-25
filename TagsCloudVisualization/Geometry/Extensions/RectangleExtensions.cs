﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TagsCloudVisualization.Geometry
{
    public static class RectangleExtensions
    { 
        // !CR (krait): Где тесты на это?

        //по-английски это описанный прямоугольник 0_о
        public static Rectangle CoveringRectangle(this IEnumerable<Rectangle> rectangles)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            var exists = false;
            foreach (var rectangle in rectangles)
            {
                exists = true;
                minX = Math.Min(rectangle.Left, minX);
                minY = Math.Min(rectangle.Bottom, minY);

                maxX = Math.Max(rectangle.Right, maxX);
                maxY = Math.Max(rectangle.Top, maxY);
            }
            return !exists ? Rectangle.FromCentre(Vector.Zero, Size.Empty) : new Rectangle(maxX, maxY, minX, minY);
        }
    }
}