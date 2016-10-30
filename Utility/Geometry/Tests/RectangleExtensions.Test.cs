using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Utility.Geometry.Extensions;

namespace Utility.Geometry.Tests
{
    [TestFixture]
    public class RectangleExtensions_CoveringRectangle_Should
    {
        private Rectangle[] rectangles;

        [SetUp]
        public void SetUp()
        {
            rectangles = new[] { new Rectangle(0, 0, 100, 100), new Rectangle(-10, 304, 50, -100600), new Rectangle(0, 0, 0, 0) };
        }


        [Test]
        public void CoverAllRectangles()
        {
            var coveringRectangle = rectangles.CoveringRectangle();
            rectangles.Count(r => !coveringRectangle.Contains(r)).Should().Be(0);
        }

        [Test]
        public void BeEmpty_ForZeroRectangles()
        {
            var coveringRectangle = Enumerable.Empty<Rectangle>().CoveringRectangle();
            coveringRectangle.Should().Be(Rectangle.Empty);
        }
    }
}