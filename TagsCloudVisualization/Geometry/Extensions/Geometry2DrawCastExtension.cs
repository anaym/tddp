using System.Drawing;

namespace TagsCloudVisualization.Geometry.Extensions
{
    public static class Geometry2DrawCastExtension
    {
        public static RectangleF Transform(this VectorTransform transform, Rectangle rectangle)
        {
            var a = transform.Transform(rectangle.LeftTop);
            //var a = rectangle.LeftTop.ToPointF(tangentialRectangle);
            // !CR (krait): Зачем нужна переменная b?
            return new RectangleF(a.X, a.Y, rectangle.Size.Width, rectangle.Size.Height);
        }

        // !CR (krait): Rectangle outter? Попробуй назвать этот параметр так, чтобы было понятно, что он значит.
        public static Color ToColor(this object obj) => Color.FromArgb(255, Color.FromArgb(obj.GetHashCode()));
    }
}