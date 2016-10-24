using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    public static class Geometry2DrawCastExtension
    {
        public static RectangleF ToRectangleF(this Rectangle rectangle, Rectangle tangentialRectangle)
        {
            var a = rectangle.LeftTop.ToPointF(tangentialRectangle);
            // !CR (krait): Зачем нужна переменная b?
            return new RectangleF(a.X, a.Y, rectangle.Size.Width, rectangle.Size.Height);
        }

        // !CR (krait): Rectangle outter? Попробуй назвать этот параметр так, чтобы было понятно, что он значит.
        public static PointF ToPointF(this Vector vector, Rectangle outter)
        {
            return new PointF(vector.X - outter.Left, outter.Size.Height - vector.Y + outter.Bottom);
        }

        public static Color ToColor(this object obj) => Color.FromArgb(255, Color.FromArgb(obj.GetHashCode()));
    }
}