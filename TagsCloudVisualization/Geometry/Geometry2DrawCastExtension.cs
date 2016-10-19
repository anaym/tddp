using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    public static class Geometry2DrawCastExtension
    {
        public static Rectangle ToRectangle(this RectangleF tangentialRectangle)
        {
            return new Rectangle(new Vector((int)tangentialRectangle.Left, (int)tangentialRectangle.Bottom), new Vector((int)tangentialRectangle.Right, (int)tangentialRectangle.Top));
        }

        public static RectangleF ToRectangleF(this Rectangle rectangle, Rectangle tangentialRectangle)
        {
            var a = rectangle.LeftTop.ToPointF(tangentialRectangle);
            var b = rectangle.RightBottom.ToPointF(tangentialRectangle);
            return new RectangleF(a.X, a.Y, rectangle.Size.Width, rectangle.Size.Height);
        }

        public static PointF ToPointF(this Vector vector, Rectangle outter)
        {
            return new PointF(vector.X - outter.Left, outter.Size.Height - vector.Y + outter.Bottom);
        }

        public static Color ToColor(this object obj) => Color.FromArgb(255, Color.FromArgb(obj.GetHashCode()));
    }
}