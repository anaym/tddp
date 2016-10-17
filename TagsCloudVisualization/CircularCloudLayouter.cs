using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Vector Centre;
        private List<Rectangle> rectangles;

        public CircularCloudLayouter(Vector centre)
        {
            this.Centre = centre;
            rectangles = new List<Rectangle>();
        }

        public CircularCloudLayouter() : this(new Vector(0, 0))
        { }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Any() &&
                (rectangles.Last().Size.Width < rectangleSize.Width ||
                 rectangles.Last().Size.Height < rectangleSize.Height))
            {
                throw new ArgumentException("Rectangles must be ordered by desending sizes");
            }

            var rect = new Rectangle(rectangleSize, Centre);
            rectangles.Add(rect);
            return rect;
        }

        public IEnumerable<Rectangle> GetRectangles() => rectangles;
    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
        }


        [TestCase(0, 0, TestName = "Begin of coordinates")]
        [TestCase(100500, 10, TestName = "Another point")]
        public void PutFirstRectangleToLayoutCentre(int cx, int cy)
        {
            layouter = new CircularCloudLayouter(new Vector(cx, cy));
            var rect = layouter.PutNextRectangle(new Size(100, 20));
            rect.Centre.Should().Be(layouter.Centre);
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

        [Test]
        public void SaveAllPuttedRectangles()
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
    }
}
