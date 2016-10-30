using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace Utility.Geometry.Tests
{
    [TestFixture]
    public class VectorTransform_Should
    {
        private Vector b;
        private Vector a;
        private Rectangle cover;
        private VectorCoordinateSystemConverter coordinateSystemConverter;

        [SetUp]
        public void SetUp()
        {
            a = new Vector(-100, 50);
            b = new Vector(32, 45);
            cover = new Rectangle(50, 50, -100, -100);
            coordinateSystemConverter = new VectorCoordinateSystemConverter(cover);
        }


        [Test]
        public void NotChangeDistanceBetweenVectors()
        {
            var ta = coordinateSystemConverter.Transform(a);
            var tb = coordinateSystemConverter.Transform(b);
            var delta = new PointF(tb.X - ta.X, tb.Y - ta.Y);
            ((int) Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y)).Should().Be(a.DistanceTo(b));
        }
    }
}