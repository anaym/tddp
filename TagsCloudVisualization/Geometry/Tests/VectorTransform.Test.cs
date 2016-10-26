using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    [TestFixture]
    public class VectorTransform_Should
    {
        private Vector b;
        private Vector a;
        private Rectangle cover;
        private VectorTransform transform;

        [SetUp]
        public void SetUp()
        {
            a = new Vector(-100, 50);
            b = new Vector(32, 45);
            cover = new Rectangle(50, 50, -100, -100);
            transform = new VectorTransform(cover);
        }


        [Test]
        public void NotChangeDistanceBetweenVectors()
        {
            var ta = transform.Transform(a);
            var tb = transform.Transform(b);
            var delta = new PointF(tb.X - ta.X, tb.Y - ta.Y);
            ((int) Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y)).Should().Be(a.DistanceTo(b));
        }
    }
}