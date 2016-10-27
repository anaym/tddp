using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    // CR (krait): Не вышло, попробуй ещё раз. Обрати внимание также на имена самих тестов, а не только кейсов.
    // !CR (krait): Поправь имена в соответствии с грамматикой английского языка. Примеры в других файлах с тестами.
    [TestFixture]
    public class ParallelSegment_Should
    {
        [TestCase(0, 1, 1, 2, true, TestName = "other parallel segment in point")]
        [TestCase(0, 2, 1, 3, true, TestName = "other parallel segment in segment")]
        [TestCase(0, 2, 1, 3, false, TestName = "other parallel segment in segment, when excluding border")]
        [TestCase(0, 2, 0, 2, true, TestName = "equal rectangle")]
        [TestCase(0, 1, 0, 1, false, TestName = "equal rectangle, excluding border")]
        public void Intersected_With(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeTrue();
        }

        [TestCase(0, 1, -1, -2, true, TestName = "remote rectangle")]
        [TestCase(0, 1, 1, 2, false, TestName = "other parallel segment in point, when excluding border")]
        public void NotIntersected_With(int leftA, int rightA, int leftB, int rightB, bool includeBorder)
        {
            new ParallelSegment(leftA, rightA).IsIntersected(new ParallelSegment(leftB, rightB), includeBorder).Should().BeFalse();
        }

        [TestCase(0, 2, 1, true, TestName = "point inside")]
        [TestCase(0, 2, 1, false, TestName = "point inside, when excluding borders")]
        [TestCase(0, 2, 0, true, TestName = "point in border")]
        public void ContainsPoint_When(int left, int right, int point, bool includeBorder)
        {
            new ParallelSegment(left, right).Contains(point, includeBorder).Should().BeTrue();
        }

        [TestCase(0, 2, 0, false, TestName = "point in border, when excluding border")]
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