using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    [TestFixture]
    public class Rectangle_Should
    {
        #region [TestCases]

        // CR (krait): Названия кейсов всё ещё непонятные.

        [TestCase(0, 0, 100, 0, true, TestName = "Touching on line")]
        [TestCase(0, 0, 100, 50, true, TestName = "Touching on point")]

        [TestCase(0, 0, 0, 0, true, TestName = "Equals")]
        [TestCase(110, 10, 0, 0, true, 2, TestName = "Nested")]

        [TestCase(0, 0, 50, 0, true, TestName = "Overlap parallel")]
        [TestCase(0, 0, 50, 10, true, TestName = "Overlap not parallel")]

        [TestCase(0, 0, 0, 0, false, TestName = "Equals without include contour")]
        [TestCase(110, 10, 0, 0, false, 2, TestName = "Nested without include contour")]

        [TestCase(0, 0, 50, 0, false, TestName = "Overlap parallel without include contour")]
        [TestCase(0, 0, 50, 10, false, TestName = "Overlap not parallel without include contour")]

        #endregion
        public void BeIntersected_When(int xA, int yA, int xB, int yB, bool includeContour, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContour)
                .Should().BeTrue();
        }

        #region [TestCases]

        // CR (krait): Названия кейсов всё ещё непонятные.

        [TestCase(0, 0, 100, 0, false, TestName = "Touching on line without include contour")]
        [TestCase(0, 0, 100, 50, false, TestName = "Touching on point without include contour")]

        [TestCase(0, 0, 1000, 50, true, TestName = "Spaced")]
        [TestCase(0, 0, 1000, 50, false, TestName = "Spaced without include contour")]

        #endregion
        public void NotBeIntersected_When(int xA, int yA, int xB, int yB, bool includeContour, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContour)
                .Should().BeFalse();
        }

        // CR (krait): Вообще границу геометрической фигуры принято называть border.
        [TestCase(-10, -20, false, TestName = "point is inside rectangle (without contours)")]
        [TestCase(0, 0, true, TestName = "point is on rectangle's contour")]
        public void ContainPoint_When(int x, int y, bool includeContour)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeContour).Should().BeTrue();
        }

        [TestCase(100, -2000, true, TestName = "point is outside rectangle")]
        [TestCase(0, 0, false, TestName = "point is on rectangle's contour")]
        public void NotContainPoint_When(int x, int y, bool includeContour)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeContour).Should().BeFalse();
        }

        [TestCase(0, 0, 0, 0, TestName = "Other rect is inside this rectangle")]
        public void ContainOtherRectangle_When(int xA, int yA, int xB, int yB)
        {
            var self = new Rectangle(new Vector(xA, yA), new Size(100, 125));
            var other = new Rectangle(new Vector(xB, yB), new Size(20, 25));
            self.Contains(other).Should().BeTrue();
        }

        [TestCase(0, 0, 1000, -1000, TestName = "Other rect  is outside this rectangle")]
        [TestCase(0, 0, 0, 10, TestName = "Other rect is intersected with this rectangle")]
        public void NotContainOtherRectangle_When(int xA, int yA, int xB, int yB)
        {
            var self = new Rectangle(new Vector(xA, yA), new Size(100, 125));
            var other = new Rectangle(new Vector(xB, yB), new Size(20, 25));
            self.Contains(other).Should().BeFalse();
        }

        [Test]
        public void HaveEqualHash_WithEqualRectangle()
        {
            var a = new Rectangle(new Random().Next(10), new Random().Next(10), new Random().Next(10), new Random().Next(10));
            var b = new Rectangle(a.RightTop, a.Size);
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equal_OtherCreatedFromSameArguments()
        {
            var a = new Rectangle(new Random().Next(10), new Random().Next(10), new Random().Next(10), new Random().Next(10));
            var b = new Rectangle(a.RightTop, a.Size);
            a.Should().Be(b);
        }

        [Test]
        public void NotEqual_OtherCreatedFromAnotherArguments()
        {
            var a = new Rectangle(new Random().Next(10), new Random().Next(10), new Random().Next(10), new Random().Next(10));
            var b = new Rectangle(a.RightTop, new Size(100500, 234));
            a.Should().NotBe(b);
        }
    }
}