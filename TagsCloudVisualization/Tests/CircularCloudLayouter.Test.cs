using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Tests
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
            var rnd = new Random(123);
            for (int i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next() % 10000, count - i + minSize - 1);
                var rect = layouter.PutNextRectangle(size);
                buffer.Add(rect);
            }
            return buffer;
        }

        private int GetOutRectangleSquare()
        {
            return layouter.GetRectangles().TangentialRectangle().Size.Square;
        }

        private int GetSumPiecesSquare()
        {
            if (!layouter.GetRectangles().Any())
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

        [TestCase(5, 10, 7, 12, TestName = "All dimensions")]
        [TestCase(5, 10, 3, 12, TestName = "Non comparable")]
        public void ThrowArgumentException_WhenHeightOrderedBySizeDecending(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [TestCase(5, 10, 30, 8, TestName = "Non comparable")]
        public void CorrectWork_WhenWidthNotOrderedByDecending(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.DoesNotThrow(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [Test]
        public void ThrowNullArgumentException_WhenPutNull()
        {
            Assert.Throws<ArgumentNullException>(() => layouter.PutNextRectangle(null));
        }

        [Test]
        public void RectanglesMustNotIntersected_AfterPut()
        {
            PutAnyRectangles(100);
            layouter
                .GetRectangles()
                .Where(r => layouter.GetRectangles().All(o => o.IsIntersected(r, false) && o != r))
                .ToList()
                .ShouldAllBeEquivalentTo(Enumerable.Empty<Rectangle>());
        }

        [Test]
        public void TightlyPlaceRectangles()
        {
            PutAnyRectangles(10);
            Math.Abs(GetSumPiecesSquare() - GetOutRectangleSquare()).Should().BeLessThan((int)(0.8 * GetSumPiecesSquare()));
        }
    }
}