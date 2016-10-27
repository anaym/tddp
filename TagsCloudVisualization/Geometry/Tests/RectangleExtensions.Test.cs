using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    // @CR (krait): Тут название класса ни о чём не говорит. Более того, там может быть несколько методов, отвечающих за абсолютно разные вещи. Надо как-то добавить имя метода в имя fixture.
    [TestFixture]
    public class CoveringRectangle_Should
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