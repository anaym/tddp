using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    public class VectorCoordinateSystemConverter
    {
        public readonly Rectangle FrameRectangle;

        public VectorCoordinateSystemConverter(Rectangle frameRectangle)
        {
            FrameRectangle = frameRectangle;
        }

        public PointF Transform(Vector vector)
        {
            return new PointF(vector.X - FrameRectangle.Left, FrameRectangle.Size.Height - vector.Y + FrameRectangle.Bottom);
        }
    }
}