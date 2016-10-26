﻿using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace TagsCloudVisualization.Geometry
{
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