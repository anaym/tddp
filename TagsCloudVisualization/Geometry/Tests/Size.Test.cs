using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Geometry.Tests
{
    [TestFixture]
    public class Size_Should
    {
        private Size size;

        [SetUp]
        public void SetUp()
        {
            size = new Size(new Random().Next(0, 100), new Random().Next(0, 100));
        }


        [Test]
        public void CalculateArea_Correct()
        {
            size.Area.Should().Be(size.Width*size.Height);
        }

        [Test]
        public void ThrowArgumentException_WhenNegativeDimension()
        {
            Assert.Throws<ArgumentException>(() => new Size(-1, 0));
        }
    }
}