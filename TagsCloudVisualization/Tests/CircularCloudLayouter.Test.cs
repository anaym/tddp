using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Tests
{
    // CR (krait): Проблемы с именованием тестов. См. комментарий к ParallelSegment_Should.

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
        }

        [TearDown]
        public void Failure()
        {
            Console.WriteLine("Fail");
        }

        private List<Rectangle> PutSomeRectangles(int count, int minSize = 1)
        {
            var buffer = new List<Rectangle>();
            var rnd = new Random(123);
            for (int i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(10000), count - i + minSize - 1);
                var rect = layouter.PutNextRectangle(size);
                buffer.Add(rect);
            }
            return buffer;
        }

        private int GetOuterRectangleArea()
        {
            return layouter.GetRectangles().TangentialRectangle().Size.Area;
        }

        private int GetTotalAreaOfRectangles()
        {
            if (!layouter.GetRectangles().Any())
                return 0;
            // !CR (krait): В Size же есть хелпер, считающий площадь?
            return layouter.GetRectangles().Sum(r =>r.Size.Area);
        }

        [TestCase(0, 0, TestName = "Begin of coordinates")]
        [TestCase(100500, 10, TestName = "Another point")]
        public void PutFirstRectangleToCentre(int cx, int cy)
        {
            layouter = new CircularCloudLayouter(new Vector(cx, cy));
            var rect = layouter.PutNextRectangle(new Size(100, 20));
            rect.Centre.Should().Be(layouter.Centre);
        }

        // !CR (krait): Put - неправильный глагол.
        [Test]
        public void Remember_AllRectanglesSize()
        {
            var putted = PutSomeRectangles(10).Select(r => r.Size);
            layouter.GetRectangles()
                .Select(r => r.Size)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [Test]
        public void NotMove_PreviousPlacedRectangles()
        {
            var putted = PutSomeRectangles(10, minSize:2);

            layouter.PutNextRectangle(new Size(1, 1));

            layouter.GetRectangles()
                .Take(10)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [TestCase(5, 10, 7, 12, TestName = "All dimensions")]
        [TestCase(5, 10, 3, 12, TestName = "Non comparable")]
        public void NotThrows_WhenSizesNotOrdered(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.DoesNotThrow<ArgumentException>(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [TestCase(5, 10, 30, 8, TestName = "Non comparable")]
        public void CorrectWork_WhenWidthNotOrderedByDecending(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.DoesNotThrow(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [Test]
        public void RectanglesMustNotIntersected_AfterPut()
        {
            PutSomeRectangles(100);
            layouter
                .GetRectangles()
                .Where(r => layouter.GetRectangles().All(o => o.IsIntersected(r, false) && !o.Equals(r)))
                .ToList()
                .ShouldAllBeEquivalentTo(Enumerable.Empty<Rectangle>());
        }

        [Test]
        public void TightlyPlaceRectangles()
        {
            PutSomeRectangles(10);
            Math.Abs(GetTotalAreaOfRectangles() - GetOuterRectangleArea()).Should().BeLessThan((int)(0.8 * GetTotalAreaOfRectangles()));
        }
    }
}