using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TagsCloudVisualization.Geometry
{
    public static class RectangleExtensions
    {
        public static Rectangle OutRectangle(this IEnumerable<Rectangle> rectangles)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MaxValue;
            var exist = false;
            foreach (var rectangle in rectangles)
            {
                exist = true;
                minX = Math.Min(rectangle.LeftBottom.X, minX);
                minY = Math.Min(rectangle.LeftBottom.Y, minY);

                maxX = Math.Max(rectangle.RightTop.X, maxX);
                maxY = Math.Max(rectangle.RightTop.Y, maxY);
            }
            return !exist ? null : new Rectangle(new Vector(maxX, maxY), new Vector(minX, minY));
        }

        public static int GetBorder(this Rectangle rect, Direction border)
        {
            switch (border)
            {
                case Direction.Up:
                    return rect.RightTop.Y;
                case Direction.Down:
                    return rect.LeftBottom.Y;
                case Direction.Left:
                    return rect.LeftBottom.X;
                case Direction.Right:
                    return rect.RightTop.X;
                default:
                    throw new ArgumentOutOfRangeException(nameof(border), border, null);
            }
        }

        public static void SetBorder(this Rectangle rect, Direction border, int value)
        {
            switch (border)
            {
                case Direction.Up:
                    rect.RightTop = new Vector(rect.RightTop.X, value);
                    break;
                case Direction.Down:
                    rect.LeftBottom = new Vector(rect.LeftBottom.X, value);
                    break;
                case Direction.Left:
                    rect.LeftBottom = new Vector(value, rect.LeftBottom.Y);
                    break;
                case Direction.Right:
                    rect.RightTop = new Vector(value, rect.RightTop.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(border), border, null);
            }
        }
    }
}