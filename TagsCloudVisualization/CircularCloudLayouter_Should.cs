using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
        }

        private List<Rectangle> PutAnyRectangles(int count, int minSize = 1)
        {
            var buffer = new List<Rectangle>();
            for (int i = 0; i < count; i++)
            {
                var size = new Size(count - i + minSize - 1, count - i + minSize - 1);
                var rect = layouter.PutNextRectangle(size);
                buffer.Add(rect);
            }
            return buffer;
        }

        private int GetOutRectangleSquare()
        {
            return layouter.GetRectangles().OutRectangle().Size.Square;
        }

        private int GetSumPiecesSquare()
        {
            if (!layouter.rectangles.Any())
                return 0;
            return layouter.GetRectangles().Sum(r => r.Size.Width*r.Size.Height);
        }

        [TestCase(0, 0, TestName = "Begin of coordinates")]
        [TestCase(100500, 10, TestName = "Another point")]
        public void PutFirstRectangleToLayoutCentre(int cx, int cy)
        {
            layouter = new CircularCloudLayouter(new Vector(cx, cy));
            var rect = layouter.PutNextRectangle(new Size(100, 20));
            rect.Centre.Should().Be(layouter.Centre);
        }

        [Test]
        public void SaveAllPuttedSizes()
        {
            var putted = PutAnyRectangles(10).Select(r => r.Size);
            layouter.GetRectangles()
                .Select(r => r.Size)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [Test]
        public void DontMutatePreviousRectangles_AfterPutNew()
        {
            var putted = PutAnyRectangles(10, minSize:2);

            layouter.PutNextRectangle(new Size(1, 1));

            layouter.GetRectangles()
                .Take(10)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [Test]
        public void ThrowArgumentException_WhenRectanclesPuttedNonOrderedBySizeDecending()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(100, 100)));
        }

        [Test]
        public void ThrowNullArgumentException_WhenTryPuttNull()
        {
            Assert.Throws<ArgumentNullException>(() => layouter.PutNextRectangle(null));
        }

        [Test]
        public void DeployRectangleNonIntersected()
        {
            PutAnyRectangles(10);
            layouter
                .GetRectangles()
                .All(r => layouter.GetRectangles().All(o => !o.IsIntersected(r, false)))
                .Should().BeTrue();
        }

        [Test]
        public void TestEfficency()
        {
            PutAnyRectangles(10);
            Math.Abs(GetSumPiecesSquare() - GetOutRectangleSquare()).Should().BeLessThan((int)(0.8 * GetOutRectangleSquare()));
        }
    }
}