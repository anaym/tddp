using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    // CR (krait): Проблемы с именованием тестов. См. комментарий к ParallelSegment_Should.

    // !CR (krait): Где тесты на Contains?

    [TestFixture]
    public class Rectangle_Should
    {
        #region [TestCases]
        [TestCase(0, 0, 100, 0, true, TestName = "Touching on line")]
        [TestCase(0, 0, 100, 50, true, TestName = "Touching on point")]

        [TestCase(0, 0, 0, 0, true, TestName = "Equals")]
        [TestCase(110, 10, 0, 0, true, 2, TestName = "Nested")]

        [TestCase(0, 0, 50, 0, true, TestName = "Overlap parallel")]
        [TestCase(0, 0, 50, 10, true, TestName = "Overlap not parallel")]

        [TestCase(0, 0, 0, 0, false, TestName = "Equals without include contur")]
        [TestCase(110, 10, 0, 0, false, 2, TestName = "Nested without include contur")]

        [TestCase(0, 0, 50, 0, false, TestName = "Overlap parallel without include contur")]
        [TestCase(0, 0, 50, 10, false, TestName = "Overlap not parallel without include contur")]
        #endregion
        public void Intersected_When(int xA, int yA, int xB, int yB, bool includeContur, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContur)
                .Should().BeTrue();
        }

        #region [TestCases]
        [TestCase(0, 0, 100, 0, false, TestName = "Touching on line without include contur")]
        [TestCase(0, 0, 100, 50, false, TestName = "Touching on point without include contur")]

        [TestCase(0, 0, 1000, 50, true, TestName = "Spaced")]
        [TestCase(0, 0, 1000, 50, false, TestName = "Spaced without include contur")]
        #endregion
        public void NotIntersected_When(int xA, int yA, int xB, int yB, bool includeContur, int scaleA = 1)
        {
            var size = new Size(100, 50);
            Rectangle.FromRightTop(new Vector(xA, yB), (size.ToVector() * scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContur)
                .Should().BeFalse();
        }

        [TestCase(-10, -20, false, TestName = "point in rectangle (without conturs)")]
        [TestCase(0, 0, true, TestName = "point on rectangle`s contur")]
        public void ContainsPoint_When(int x, int y, bool includeContur)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeContur).Should().BeTrue();
        }

        [TestCase(100, -2000, true, TestName = "point out rectangle")]
        [TestCase(0, 0, false, TestName = "point on rectangle`s contur (without conturs)")]
        public void NotContainsPoint_When(int x, int y, bool includeContur)
        {
            var self = new Rectangle(new Vector(0, 0), new Size(100, 125));
            self.Contains(new Vector(x, y), includeContur).Should().BeFalse();
        }

        [TestCase(0, 0, 0, 0, TestName = "Other rect in this rectangle")]
        public void ContainsOtherRectangle_When(int xA, int yA, int xB, int yB)
        {
            var self = new Rectangle(new Vector(xA, yA), new Size(100, 125));
            var other = new Rectangle(new Vector(xB, yB), new Size(20, 25));
            self.Contains(other).Should().BeTrue();
        }

        [TestCase(0, 0, 1000, -1000, TestName = "Other rect out this rectangle")]
        [TestCase(0, 0, 0, 10, TestName = "Other rect intersected with this rectangle")]
        public void NotContainsOtherRectangle_When(int xA, int yA, int xB, int yB)
        {
            var self = new Rectangle(new Vector(xA, yA), new Size(100, 125));
            var other = new Rectangle(new Vector(xB, yB), new Size(20, 25));
            self.Contains(other).Should().BeFalse();
        }
    }
}