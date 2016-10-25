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
        public void CalculateLength_Correct()
        {
            vectorA.Norm.Should().Be((int) Math.Sqrt(vectorA.X*vectorA.X + vectorA.Y*vectorA.Y));
        }

        [Test]
        public void CalculateDistance_Correct()
        {
            var expected = (int)Math.Sqrt(Math.Pow(vectorB.X-vectorA.X, 2) + Math.Pow(vectorB.Y - vectorA.Y, 2));
            vectorA.DistanceTo(vectorB).Should().Be(expected);
        }

        [Test]
        public void Sum_Correct()
        {
            var res = vectorA + vectorB;
            res.Should().Be(new Vector(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y));
        }

        [Test]
        public void Sub_Correct()
        {
            var res = vectorA - vectorB;
            res.Should().Be(new Vector(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y));
        }

        [Test]
        public void MulToScalar_Correct()
        {
            var res = vectorA * 4;
            res.Should().Be(new Vector(vectorA.X * 4, vectorA.Y * 4));
        }

        [Test]
        public void DivToScalar_Correct()
        {
            var res = vectorA / 4;
            res.Should().Be(new Vector(vectorA.X / 4, vectorA.Y / 4));
        }

        [Test]
        public void Inverse_Correct()
        {
            var res = -vectorA;
            res.Should().Be(new Vector(-vectorA.X, -vectorA.Y));
        }
    }
}