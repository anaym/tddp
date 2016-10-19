using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Test
{
    [TestFixture]
    public class ParallelSegment_Should
    {
        [TestCase(0, 1, 1, 2, true, TestName = "Touching")]
        [TestCase(1, 2, 0, 1, true, TestName = "Reverse touching")]
        [TestCase(0, 2, 1, 3, true, TestName = "Overlap")]
        [TestCase(0, 2, 1, 3, false, TestName = "Overlap without include border")]
        [TestCase(0, 1, 0, 1, false, TestName = "Equals without include border")]
        [TestCase(0, 2, 0, 2, true, TestName = "Equals")]
        public void Intersected_When(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeTrue();
        }

        [TestCase(0, 1, -1, -2, true, TestName = "Spaced")]
        [TestCase(0, 1, 1, 2, false, TestName = "Touching without include border")]
        [TestCase(1, 2, 0, 1, false, TestName = "Reverse touching without include border")]
        public void NotIntersected_When(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeFalse();
        }
    }
}