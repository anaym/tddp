using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    [TestFixture]
    public class Vector_Should
    {
        private Vector vectorA;
        private Vector vectorB;

        [SetUp]
        public void SetUp()
        {
            vectorA = new Vector(new Random().Next(0, 100), new Random().Next(0, 100));
            vectorB = new Vector(new Random().Next(0, 100), new Random().Next(0, 100));
        }

        [Test]
        public void CalculateLength_Correctly()
        {
            vectorA.Norm.Should().Be((int)Math.Sqrt(vectorA.X * vectorA.X + vectorA.Y * vectorA.Y));
        }

        [Test]
        public void CalculateDistance_Correctly()
        {
            var expected = (int)Math.Sqrt(Math.Pow(vectorB.X - vectorA.X, 2) + Math.Pow(vectorB.Y - vectorA.Y, 2));
            vectorA.DistanceTo(vectorB).Should().Be(expected);
        }

        [Test]
        public void Sum_Correctly()
        {
            var res = vectorA + vectorB;
            res.Should().Be(new Vector(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y));
        }

        [Test]
        public void Subtract_Correctly()
        {
            var res = vectorA - vectorB;
            res.Should().Be(new Vector(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y));
        }

        [Test]
        public void MultiplyToScalar_Correctly()
        {
            var res = vectorA * 4;
            res.Should().Be(new Vector(vectorA.X * 4, vectorA.Y * 4));
        }

        [Test]
        public void DivideToScalar_Correctly()
        {
            var res = vectorA / 4;
            res.Should().Be(new Vector(vectorA.X / 4, vectorA.Y / 4));
        }

        [Test]
        public void Invert_Correctly()
        {
            var res = -vectorA;
            res.Should().Be(new Vector(-vectorA.X, -vectorA.Y));
        }

        [Test]
        public void HaveEqualHash_WithEqualVector()
        {
            var a = new Vector(new Random().Next(10), new Random().Next(10));
            var b = new Vector(a.X, a.Y);
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equal_SimilarlyCreatedSegment()
        {
            var a = new Vector(new Random().Next(10), new Random().Next(10));
            var b = new Vector(a.X, a.Y);
            a.Should().Be(b);
        }

        [Test]
        public void NotEqual_NotSimilarlyCreatedSegment()
        {
            var a = new Vector(new Random().Next(10), new Random().Next(10));
            var b = new Vector(a.X, a.Y + 100500);
            a.Should().NotBe(b);
        }
    }
}