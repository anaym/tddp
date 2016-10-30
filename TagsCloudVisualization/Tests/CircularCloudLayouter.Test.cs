using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Utility;
using Utility.Geometry;
using Utility.Geometry.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        #region Service
        private CircularCloudLayouter layouter;
        private string failureLogDirectoryPath;
        
        [OneTimeSetUp]
        public void FailureLoggingSetup()
        {
            failureLogDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "fails out");
            if (Directory.Exists(failureLogDirectoryPath))
                Directory.Delete(failureLogDirectoryPath, true);
            Directory.CreateDirectory(failureLogDirectoryPath);
        }
        
        [TearDown]
        public void FailureLogging()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                try
                {
                    var filePath = Path.Combine(failureLogDirectoryPath, TestContext.CurrentContext.Test.FullName) + ".png";
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

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
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
        #endregion

        [Ignore("Because example")]
        [Test]
        public void FailExample()
        {
            PutSomeRectangles(10);
            
            Assert.Fail("Example :)");
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
                .Should().Equal(put);
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
        [TestCase(5, 10, 3, 12, TestName = "Width ordered")]
        [TestCase(5, 10, 30, 8, TestName = "Height ordered")] //double
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
                .Should().BeEmpty();
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