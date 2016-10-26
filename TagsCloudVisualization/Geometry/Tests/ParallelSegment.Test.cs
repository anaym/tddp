using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    // !CR (krait): 
    // Проблемы с именованием тестов. 
    // По твоей идее, как я понял, название TestFixture и название теста должны вместе образовывать фразу, задающую свойство, которое должно выполняться. 
    // Однако, это не очень получается: ParallelSegment should intersected when equals - это не по-английски.
    // Попробуй поменять названия так, чтобы они образовывали корректные фразы на английском, а то сейчас читается очень тяжело.

    // !CR (krait): Где тесты на Contains?

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

        [TestCase(0, 2, 1, true, TestName = "Centre with borders")]
        [TestCase(0, 2, 1, false, TestName = "Centre without borders")]
        [TestCase(0, 2, 0, true, TestName = "Left with border")]
        [TestCase(0, 2, 2, true, TestName = "Right with border")]
        public void ContainsPoint_When(int left, int right, int point, bool includeBorder)
        {
            new ParallelSegment(left, right).Contains(point, includeBorder).Should().BeTrue();
        }

        [TestCase(0, 2, 0, false, TestName = "Left without border")]
        [TestCase(0, 2, 2, false, TestName = "Right without border")]
        [TestCase(0, 2, -2, true, TestName = "Out with border")]
        [TestCase(0, 2, -2, false, TestName = "Out without border")]
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

        [Test]
        public void Equal_OtherCreatedFromSameArguments()
        {
            var a = new ParallelSegment(new Random().Next(10), new Random().Next(10));
            var b = new ParallelSegment(a.Left, a.Right);
            a.Should().Be(b);
        }

        [Test]
        public void NotEqual_OtherCreatedFromAnotherArguments()
        {
            var a = new ParallelSegment(new Random().Next(10), new Random().Next(10));
            var b = new ParallelSegment(a.Left, 100500 - a.Right);
            a.Should().NotBe(b);
        }
    }
}