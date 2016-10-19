using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Test
{
    [TestFixture]
    public class Rectangle_Should
    {
        [Test]
        public void CreateCorrect()
        {
            var rect = Rectangle.FromRightTop(new Vector(100, 0), new Size(100, 50));
            rect.Size.Should().Be(new Size(100, 50));
        }

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
            Rectangle.FromRightTop(new Vector(xA, yB), size.ToVector().Mul(scaleA).ToSize())
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
            Rectangle.FromRightTop(new Vector(xA, yB), size.ToVector().Mul(scaleA).ToSize())
                .IsIntersected(Rectangle.FromRightTop(new Vector(xB, yB), size), includeContur)
                .Should().BeFalse();
        }
    }
}