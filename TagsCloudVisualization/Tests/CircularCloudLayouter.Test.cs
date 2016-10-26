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
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        // CR (krait): Можно и поконкретнее назвать.
        private static readonly string DirPath;

        // CR (krait): Для этого есть TestFixtureSetUp.
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

        // CR (krait): Это не всегда Failure(), а только если Status == TestStatus.Failed. 
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

            for (int i = 0; i < count; i++)
            {
                var size = new Size(4 * (count - i + minSize - 1), 2 * (count - i + minSize - 1));
                var rect = layouter.PutNextRectangle(size);
                buffer.Add(rect);
            }
            return buffer;
        }

        private int GetCoveringRectangleArea()
        {
            return layouter.Rectangles.CoveringRectangle().Size.Area;
        }

        private int GetTotalAreaOfRectangles()
        {
            if (!layouter.Rectangles.Any())
                return 0;

            return layouter.Rectangles.Sum(r => r.Size.Area);
        }

        [Ignore("Because example")]
        [Test]
        public void FailExample()
        {
            PutSomeRectangles(10);

            // CR (krait): Изящно. Хотя для этого есть Assert.Fail()
            false.Should().BeTrue();
        }

        [TestCase(0, 0, TestName = "Center of coordinates")]
        [TestCase(100500, 10, TestName = "Other point")]
        public void PlaceFirstRectangleToCentre_WhenCentrePointIs(int cx, int cy)
        {
            layouter = new CircularCloudLayouter(new Vector(cx, cy));
            var rect = layouter.PutNextRectangle(new Size(100, 20));
            rect.Centre.Should().Be(layouter.Centre);
        }
        
        [Test]
        public void Remember_AllRectanglesSize()
        {
            var put = PutSomeRectangles(10).Select(r => r.Size);
            layouter.Rectangles
                .Select(r => r.Size)
                // CR (krait): Кажется, можно попроще: .Should().Equal(put)
                .ShouldAllBeEquivalentTo(put, o => o.WithStrictOrdering());
        }

        [Test]
        public void NotMove_PreviouslyPutRectangles()
        {
            var putRectangles = PutSomeRectangles(10, minSize: 2);

            layouter.PutNextRectangle(new Size(1, 1));

            layouter.Rectangles
                .Take(10)
                .ShouldAllBeEquivalentTo(putRectangles, o => o.WithStrictOrdering());
        }

        [TestCase(5, 10, 7, 12, TestName = "All dimensions")]
        [TestCase(5, 10, 3, 12, TestName = "Non comparable")]
        // CR (krait): Хотя бы Non comparable 2 назови, чтоб они хоть как-то отличались :)
        [TestCase(5, 10, 30, 8, TestName = "Non comparable")] //double
        public void NotThrow_WhenSizesAreNotOrdered(int w1, int h1, int w2, int h2)
        {
            layouter.PutNextRectangle(new Size(w1, h1));
            Assert.DoesNotThrow(() => layouter.PutNextRectangle(new Size(w2, h2)));
        }

        [Test]
        public void PutRectangles_WithCorrectSizeRatio()
        {
            PutSomeRectangles(10);
            var coveringRectangleSize = layouter.Rectangles.CoveringRectangle().Size;
            var widthToHeightRatio = 1.0 * coveringRectangleSize.Width / coveringRectangleSize.Height;
            Math.Abs(widthToHeightRatio - 1.0 * layouter.Extension.Y / layouter.Extension.X)
                .Should().BeLessThan(widthToHeightRatio);
        }

        [Test]
        public void PutRectangles_WithoutIntersection()
        {
            PutSomeRectangles(100);
            layouter
                .Rectangles
                .Where(r => layouter.Rectangles.All(o => o.IsIntersected(r, false) && !o.Equals(r)))
                .ToList()
                // CR (krait): .Should().BeEmpty()
                .ShouldAllBeEquivalentTo(Enumerable.Empty<Rectangle>());
        }

        [Test]
        public void PutRectangles_Closely()
        {
            PutSomeRectangles(10);
            Math.Abs(GetTotalAreaOfRectangles() - GetCoveringRectangleArea())
                .Should().BeLessThan((int)(0.8 * GetTotalAreaOfRectangles()));
        }
    }
}