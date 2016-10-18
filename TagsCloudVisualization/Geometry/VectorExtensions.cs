﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Geometry
{
    public static class VectorExtensions
    {
        public static Size ToSize(this Vector leftTopPoint, Vector rightBottomPoint)
        {
            return new Size(Math.Abs(leftTopPoint.X - rightBottomPoint.X), Math.Abs(leftTopPoint.Y - rightBottomPoint.Y));
        }

        public static Size ToSize(this Vector v) => new Size(v.X, v.Y);

        public static Vector ToVector(this Size s) => new Vector(s.Width, s.Height);
    }
}