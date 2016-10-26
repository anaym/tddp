using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    // CR (krait): Слишком общее название, ни за что не догадаешься, что тут происходит, пока не прочитаешь.
    public class VectorTransform
    {
        public readonly Rectangle FrameRectangle;

        public VectorTransform(Rectangle frameRectangle)
        {
            FrameRectangle = frameRectangle;
        }

        public PointF Transform(Vector vector)
        {
            return new PointF(vector.X - FrameRectangle.Left, FrameRectangle.Size.Height - vector.Y + FrameRectangle.Bottom);
        }
    }
}