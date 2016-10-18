using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TagsCloudVisualization.Geometry
{
    public static class RectangleExtensions
    {
        //по-английски это описанный прямоугольник 0_о
        public static Rectangle TangentialRectangle(this IEnumerable<Rectangle> rectangles)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            var exist = false;
            foreach (var rectangle in rectangles)
            {
                exist = true;
                minX = Math.Min(rectangle.Left, minX);
                minY = Math.Min(rectangle.Bottom, minY);

                maxX = Math.Max(rectangle.Right, maxX);
                maxY = Math.Max(rectangle.Top, maxY);
            }
            return !exist ? null : new Rectangle(maxX, maxY, minX, minY);
        }
    }
}