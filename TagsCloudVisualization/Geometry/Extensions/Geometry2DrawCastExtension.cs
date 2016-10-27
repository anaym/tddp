﻿using System.Drawing;

namespace TagsCloudVisualization.Geometry.Extensions
{
    public static class Geometry2DrawCastExtension
    {
        public static RectangleF Transform(this VectorCoordinateSystemConverter coordinateSystemConverter, Rectangle rectangle)
        {
            var a = coordinateSystemConverter.Transform(rectangle.LeftTop);
            return new RectangleF(a.X, a.Y, rectangle.Size.Width, rectangle.Size.Height);
        }
        
        public static Color ToColor(this object obj) => Color.FromArgb(255, Color.FromArgb(obj.GetHashCode()));

        public static TagsCloudVisualization.Geometry.Size ToGeometrySize(this SizeF size) => new TagsCloudVisualization.Geometry.Size((int)size.Width, (int)size.Height);
    }
}