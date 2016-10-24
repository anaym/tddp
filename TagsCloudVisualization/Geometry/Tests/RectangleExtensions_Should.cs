using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Test
{
    [TestFixture]
    public class RectangleExtensions_Should
    {
        private Rectangle[] rectangles;

        [SetUp]
        public void SetUp()
        {
            rectangles = new[] {new Rectangle(0, 0, 100, 100), new Rectangle(-10, 304, 50, -100600), new Rectangle(0, 0, 0, 0)};
        }


        [Test]
        public void ContainsAllRectangles()
        {
            var trect = rectangles.TangentialRectangle();
            rectangles.Count(r => !trect.Contains((Rectangle) r, true)).Should().Be(0);
        }

        [Test]
        public void BeEmpty_WhenEmptyEnumeration()
        {
            var trect = Enumerable.Empty<Rectangle>().TangentialRectangle();
            trect.Should().Be(Rectangle.Empty);
        }
    }
}