using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Tests
{
    // CR (krait): Проблемы с именованием тестов. См. комментарий к ParallelSegment_Should.

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private static readonly string DirPath;

        static CircularCloudLayouter_Should()
        {
            DirPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "fails out");
            if (Directory.Exists(DirPath))
                Directory.Delete(DirPath, true);
            Directory.CreateDirectory(DirPath);
        }

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
        }

        [TearDown]
        public void Failure()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    var filePath = Path.Combine(DirPath, TestContext.CurrentContext.Test.FullName) + ".png";
                    var cloud = new TagCloud(layouter, new Size(8, 16), i => i);
                    var renderer = new TagCloudRenderer(true);
                    renderer.RenderToBitmap(cloud).Save(filePath, ImageFormat.Png);
                    TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"Can not save fail out: {ex.StackTrace}");
                }

            }
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
            return layouter.Rectangles.TangentialRectangle().Size.Area;
        }

        private int GetTotalAreaOfRectangles()
        {
            if (!layouter.Rectangles.Any())
                return 0;
            // !CR (krait): В Size же есть хелпер, считающий площадь?
            return layouter.Rectangles.Sum(r =>r.Size.Area);
        }

        [Test]
        public void FailExample()
        {
            PutSomeRectangles(10);
            false.Should().BeTrue();
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
            layouter.Rectangles
                .Select(r => r.Size)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [Test]
        public void NotMove_PreviousPlacedRectangles()
        {
            var putted = PutSomeRectangles(10, minSize:2);

            layouter.PutNextRectangle(new Size(1, 1));

            layouter.Rectangles
                .Take(10)
                .ShouldAllBeEquivalentTo(putted, o => o.WithStrictOrdering());
        }

        [TestCase(5, 10, 7, 12, TestName = "All dimensions")]
        [TestCase(5, 10, 3, 12, TestName = "Non comparable")]
        [TestCase(5, 10, 30, 8, TestName = "Non comparable")]
        public void NotThrows_WhenSizesNotOrdered(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.DoesNotThrow(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [Test]
        public void RectanglesMustNotIntersected_AfterPut()
        {
            PutSomeRectangles(100);
            layouter
                .Rectangles
                .Where(r => layouter.Rectangles.All(o => o.IsIntersected(r, false) && !o.Equals(r)))
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