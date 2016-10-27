using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    [TestFixture]
    public class Rectangle_Should
    {
        #region [TestCases]

        // !CR (krait): Названия кейсов всё ещё непонятные.

        [TestCase(0, 0, 100, 0, true, TestName = "another rectangle in one line")]
        [TestCase(0, 0, 100, 50, true, TestName = "another rectangle in one point")]

        [TestCase(0, 0, 0, 0, true, TestName = "equal rectangle")]
        [TestCase(110, 10, 0, 0, true, 2, TestName = "nested rectangle")]

        [TestCase(0, 0, 50, 0, true, TestName = "another rectangle in rectangle (Shift in x only)")]
        [TestCase(0, 0, 50, 10, true, TestName = "another rectangle in rectangle (Shift in x and y)")]

        [TestCase(0, 0, 0, 0, false, TestName = "equal rectangle, when excluding border")]
        [TestCase(110, 10, 0, 0, false, 2, TestName = "nested rectangle, when excluding border")]

        [TestCase(0, 0, 50, 0, false, TestName = "another rectangle in rectangle (Shift in x only), when excluding border")]
        [TestCase(0, 0, 50, 10, false, TestName = "another rectangle in rectangle (Shift in x and y), when excluding border")]

        #endregion
        public void BeIntersected_With(int xA, int yA, int xB, int yB, bool includeContour, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContour)
                .Should().BeTrue();
        }

        #region [TestCases]

        // !CR (krait): Названия кейсов всё ещё непонятные.

        [TestCase(0, 0, 100, 0, false, TestName = "another rectangle in one line, when excluding border")]
        [TestCase(0, 0, 100, 50, false, TestName = "another rectangle in one point, when excluding border")]

        [TestCase(0, 0, 1000, 50, true, TestName = "remote rectangle")]
        [TestCase(0, 0, 1000, 50, false, TestName = "remote rectangle, when including border")]

        #endregion
        public void NotBeIntersected_With(int xA, int yA, int xB, int yB, bool includeContour, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContour)
                .Should().BeFalse();
        }

        // !CR (krait): Вообще границу геометрической фигуры принято называть border.
        [TestCase(-10, -20, false, TestName = "point inside rectangle, when excluding border")]
        [TestCase(0, 0, true, TestName = "point in rectangle`s border")]
        public void ContainPoint_With(int x, int y, bool includeBorder)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeBorder).Should().BeTrue();
        }

        [TestCase(100, -2000, true, TestName = "point outside rectangle")]
        [TestCase(0, 0, false, TestName = "point in rectangle`s border, when excluding border")]
        public void NotContainPoint_With(int x, int y, bool includeBorder)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeBorder).Should().BeFalse();
        }

        [TestCase(0, 0, 0, 0, TestName = "other rect is inside this rectangle")]
        public void ContainOtherRectangle_When(int xA, int yA, int xB, int yB)
        {
            var self = new Rectangle(new Vector(xA, yA), new Size(100, 125));
            var other = new Rectangle(new Vector(xB, yB), new Size(20, 25));
            self.Contains(other).Should().BeTrue();
        }

        [TestCase(0, 0, 1000, -1000, TestName = "other rect is outside this rectangle")]
        [TestCase(0, 0, 0, 10, TestName = "other rect is intersected with this rectangle")]
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