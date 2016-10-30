using System;
using FluentAssertions;
using NUnit.Framework;

namespace Utility.Geometry.Tests
{
    [TestFixture]
    public class ParallelSegment_Should
    {
        [TestCase(0, 1, 1, 2, true, TestName = "another segment by point")]
        [TestCase(0, 2, 1, 3, true, TestName = "another segment by subsegment")]
        [TestCase(0, 2, 1, 3, false, TestName = "another segment by subsegment, when excluding border")]
        [TestCase(0, 2, 0, 2, true, TestName = "equal segment")]
        [TestCase(0, 1, 0, 1, false, TestName = "equal segment, excluding border")]
        public void BeIntersected_With(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeTrue();
        }

        [TestCase(0, 1, -1, -2, true, TestName = "remote segment")]
        [TestCase(0, 1, 1, 2, false, TestName = "another segment by point, when excluding border")]
        public void BeNonIntersected_With(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeFalse();
        }

        // !CR (krait): should contain, а не should contains

        [TestCase(0, 2, 1, true, TestName = "point inside")]
        [TestCase(0, 2, 1, false, TestName = "point inside and excluding borders")]
        [TestCase(0, 2, 0, true, TestName = "point in border")]
        public void ContainPoint_When(int left, int right, int point, bool includeBorder)
        {
            new ParallelSegment(left, right).Contains(point, includeBorder).Should().BeTrue();
        }

        [TestCase(0, 2, 0, false, TestName = "point in border and excluding border")]
        [TestCase(0, 2, -2, true, TestName = "point outside")]
        public void NotContainsPoint_When(int left, int right, int point, bool includeBorder)
        {
            new ParallelSegment(left, right).Contains(point, includeBorder).Should().BeFalse();
        }

        [Test]
        public void HaveEqualHash_WithEqualSegment()
        {
            var a = new ParallelSegment(new Random().Next(10), new Random().Next(10));
            var b = new ParallelSegment(a.Left, a.Right);
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        // !CR (krait): Та же проблема, что и в Rectangle_Should.

        [Test]
        public void Equal_AnotherCreatedFromSameArguments()
        {
            var a = new ParallelSegment(new Random().Next(10), new Random().Next(10));
            var b = new ParallelSegment(a.Left, a.Right);
            a.Should().Be(b);
        }

        [Test]
        public void NotEqual_AnotherCreatedFromOtherArguments()
        {
            var a = new ParallelSegment(new Random().Next(10), new Random().Next(10));
            var b = new ParallelSegment(a.Left, 100500 - a.Right);
            a.Should().NotBe(b);
        }
    }
}